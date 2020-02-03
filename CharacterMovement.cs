using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : Entity
{

    public static CharacterMovement Instance;


    [Header("Movement Settings")]
    public float JumpHeight;
    public float LerpSpeed;
    public float LerpMomentumSpeed;
    public AnimationCurve LerpMomentumCurve;
    [Range(0, 1)]
    public float MomentumThreshold;


    [Header("Look settings")]
    public Vector2 LookSpeed;
    public bool LockLook;
    public float AngleLerpMultiplier = 2;
    [Space(2)]

    [Header("ThePush")]

    public float MinPushPower;
    public float MaxPushPower;
    [Space(2)]


    public GameObject HeadMovement;
    public CharacterController controller;

    public static float VelocityMagnitude;

    private Vector3 CurrentRotation;

    private Vector3 HeadingRotation = new Vector3();

    // Start is called before the first frame update
    void Start() {
        if (Instance != null) {
            Destroy(this);
            return;
        } else {
            Instance = this;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        CurrentRotation.x = HeadMovement.transform.eulerAngles.y;

    }

    // Update is called once per frame
    void Update()
    {



        DistanceTraveledUpdate();
        MovementUpdate();

        UpdateLook();



        Vector3 HeadMovementrot = HeadMovement.transform.eulerAngles;
        HeadMovement.transform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);

    }

    public float GetFloorAngle() {

        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, -this.transform.up, out hit, 1.1f)) {

            float angle = PosNegAngle(hit.normal, this.transform.up, this.HeadMovement.transform.forward);
            return angle;

        }

        return 0;
    }

    public float PosNegAngle(Vector3 a1, Vector3 a2, Vector3 normal) {
        float angle = Vector3.Angle(a1, a2);
        float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(a1, a2)));
        return angle * sign;
    }
    //Redo momentum system to use 
    private Vector3 SlideVelocity;
    public Vector3 ConstantVelocity;
    private Vector3 MoveLerp;

    [Range(0,1)]
    public float AirDampening;
    [Range(0,1)]
    public float GroundDampening;

    bool WasGrounded = false;
    bool FirstCrouch = false;

    private Vector3 HeadingMomentum;


    private float Velocity;
    public float MinimumVelocityMomentum;
    private Vector3 LastPosition;


    private void DistanceTraveledUpdate() {


        Velocity = (LastPosition - this.transform.position).magnitude/Time.deltaTime;

        LastPosition = this.transform.position;


    }

    public bool ReleaseGravity = false;

    private void MovementUpdate() {

        Vector3 MovementDisplacement = Vector3.zero;

        bool Moved = false;

        if (Input.GetKey(KeyCode.A)) {
            Moved = true;
            MovementDisplacement.x += -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            Moved = true;
            MovementDisplacement.x += 1;
        }


        if (Input.GetKey(KeyCode.W)) {
            Moved = true;
            MovementDisplacement.z += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            Moved = true;
            MovementDisplacement.z += -1;
        }

        MovementDisplacement.Normalize();

        if (Input.GetKey(KeyCode.LeftShift)) {

            MovementDisplacement *= GlobalVars.Instance.GetMaxSpeed();
            GlobalVars.Instance.CurrentSpeed = GlobalVars.Instance.GetMaxSpeed();
            MoveLerp = MovementDisplacement;
        } else {

            if (Moved && !(Velocity < MinimumVelocityMomentum && GlobalVars.Instance.GetSpeedPercentage() > 0.25f)) {
                GlobalVars.Instance.AddSpeed();
                MovementDisplacement *= GlobalVars.Instance.CurrentSpeed;
                //Dont decay, instead increase velocity via acceleration

            } else {
                GlobalVars.Instance.DecreaseSpeed();
                MovementDisplacement *= GlobalVars.Instance.CurrentSpeed;
                //Start decaying deathdrive
            }





            if (GlobalVars.Instance.GetSpeedPercentage() > MomentumThreshold) {
                MoveLerp = Vector3.Lerp(MoveLerp, MovementDisplacement, LerpMomentumSpeed * LerpMomentumCurve.Evaluate(GlobalVars.Instance.GetSpeedPercentage()) * Time.deltaTime);
            } else {
                MoveLerp = Vector3.Lerp(MoveLerp, MovementDisplacement, LerpSpeed * Time.deltaTime);
            }
        }


        MovementDisplacement = MoveLerp;


        if (GlobalVars.Instance.GetSpeedPercentage() > MomentumThreshold) {
            HeadingMomentum.x = Mathf.Lerp(HeadingMomentum.x, CurrentRotation.x, LerpMomentumSpeed * LerpMomentumCurve.Evaluate(GlobalVars.Instance.GetSpeedPercentage()) * Time.deltaTime * AngleLerpMultiplier);

            MovementDisplacement = Quaternion.Euler(GetFloorAngle(), HeadingMomentum.x, 0) * MovementDisplacement;
        } else {

            HeadingMomentum.x = CurrentRotation.x;
            MovementDisplacement = Quaternion.Euler(GetFloorAngle(), CurrentRotation.x, 0) * MovementDisplacement;
        }



        if (controller.isGrounded) {
            LockLook = true;
            WasGrounded = true;
            if (Input.GetKeyDown(KeyCode.Space)) {
                GlobalSounds.Instance.PlayJumpSound();
                WasGrounded = false;
                ConstantVelocity.y = CalculateJumpVelocity();
            } else if(!ReleaseGravity){
                ConstantVelocity.y = -9.8f;
            }
        } else

        if (WasGrounded) {
            WasGrounded = false;

            ConstantVelocity.y = 0;
        }

        if (!controller.isGrounded) {
            LockLook = false;
        }


        if (!controller.isGrounded) {


            ConstantVelocity.x = ConstantVelocity.x * AirDampening;
            ConstantVelocity.z = ConstantVelocity.z * AirDampening;
        } else {

            ConstantVelocity.x = ConstantVelocity.x * GroundDampening;
            ConstantVelocity.z = ConstantVelocity.z * GroundDampening;
        }


        ConstantVelocity.y -= GlobalVars.Instance.gravity * Time.deltaTime;


        //print(MovementDisplacement.magnitude);


        VelocityMagnitude = MovementDisplacement.magnitude;
        MovementDisplacement += ConstantVelocity;
        
        if(VelocityMagnitude > 0.1f && controller.isGrounded && Moved && Velocity > 0.5f) {
            GlobalSounds.Instance.PlayFootsteps();
        } else {
            GlobalSounds.Instance.StopFootsteps();
        }



        //Full Charge/Slide
        if (Input.GetKey(KeyCode.LeftControl) || Physics.Raycast(this.HeadMovement.transform.position, Vector3.up, 0.51f)) {
            controller.height = 0.8f;

            if (FirstCrouch) {
                FirstCrouch = false;
                controller.Move(new Vector3(0, -0.6f, 0));
            }

        } else {
            FirstCrouch = true;
            controller.height = 2;
        }



        MovementDisplacement += SlideVelocity;

        MovementDisplacement *= Time.deltaTime;



        controller.Move(MovementDisplacement);


        

    }

    private void UpdateLook() {
        float xAxis = Input.GetAxis("Mouse X") * LookSpeed.x;
        float yAxis = Input.GetAxis("Mouse Y") * LookSpeed.y;


        if(GameSettings.Instance != null) {
            xAxis *= GameSettings.Instance.slider.value;
            yAxis *= GameSettings.Instance.slider.value;
        }

        CurrentRotation.x += xAxis;
        CurrentRotation.y += yAxis;

        if (CurrentRotation.x > 360) {
            CurrentRotation.x -= 360;
            HeadingMomentum.x -= 360;
        }
        if (CurrentRotation.x < -360) {
            CurrentRotation.x += 360;
            HeadingMomentum.x += 360;
        }
        if (CurrentRotation.y > 360) {
            CurrentRotation.y -= 360;
        }
        if (CurrentRotation.y < -360) {
            CurrentRotation.y += 360;
        }


        if (LockLook) {


            if (CurrentRotation.y < -130 || CurrentRotation.y > 140) {
                CurrentRotation.y = 0;
            }
            CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, -85, 90);

        }
    }

    public void AddImpulse(Vector3 force) {
        this.ConstantVelocity += force;
    }



    public float CalculateJumpVelocity() {
        return Mathf.Sqrt(2 * GlobalVars.Instance.gravity * JumpHeight);
    }

    private void OnCollisionEnter(Collision collision) {

    }

    public override void Pull(Vector3 Force) {
        throw new System.NotImplementedException();
    }


    public override void Damage(float amount) {
        GlobalVars.Instance.AddDeathDrive(-amount);
        print("D U M A G E");
    }
}
