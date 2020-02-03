using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingWeapon : Weapon
{
    public float damage;
    public float MaxRange;
    public Transform WeaponTip;

    public GameObject ZeStationaryGrappler;
    public Transform HookEnd;
    public GameObject moveGraplerer;
    
    public GameObject ChainLinePrefab;
    public GameObject CurrenChainLine;
    public float LineDecay;

    public float StoppingDistance;
    public float PullPower;
    public float EntityPullPower;
    public float GrapplingPower;
    public float MinimumMass;



    bool CanShoot = true;

    public override bool OnAttack() {
        Target = null;
        if (CanShoot) {
            RaycastHit hit;
            if (Physics.Raycast(GlobalPlayer.Instance.Camera.position, GlobalPlayer.Instance.Camera.transform.forward, out hit, MaxRange)) {
                CreateLine(hit.point);

                Target = hit.collider.gameObject;
                Target_Hit = hit;
                TempHitPointOffset = hit.point - hit.collider.gameObject.transform.position;

                Entity e = hit.collider.gameObject.GetComponent<Entity>();
                if (e != null) {
                    //e.Damage(damage);

                    //Force a pull on an AI Unit (Have code to initiate pull)
                    //Most likely AI Units will have to be unlocked inorder to get pulled into the player
                    //Have an entity weight included also.
                    if(e.Weight < MinimumMass) {
                        IsPullingTowardsUs = true;
                        PullingStrength = EntityPullPower;
                    } else {
                        IsPullingTowardsUs = false;
                        StartPull();
                    }


                } else {
                    Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                    if (body != null) {
                        body.AddForceAtPosition((body.gameObject.transform.position- this.WeaponTip.transform.position ).normalized * PullPower, hit.point);
                       // StartPull();//Determine if the player should be pulled or if the object is light enough to be pulled to the object

                        if(body.mass > MinimumMass) {
                            IsPullingTowardsUs = false;
                            StartPull();
                        }

                    }

                }




            } else {
                CreateLine(GlobalPlayer.Instance.Camera.position + GlobalPlayer.Instance.Camera.forward * MaxRange);
            }
                return true;
        }

        return false;
    }

    private bool IsPullingTowardsUs;
    private GameObject Target;
    private float PullingStrength;
    private RaycastHit Target_Hit;

    private Vector3 TempHitPoint;
    private Vector3 TempHitPointOffset;

    public void StartPull() {

        if (IsPullingTowardsUs) {

        } else {

            Vector3 dir = Target_Hit.point - CharacterMovement.Instance.transform.position;
            dir.Normalize();

            dir *= GrapplingPower;

            if(dir.y > 0) {

                if (CharacterMovement.Instance.ConstantVelocity.y < dir.y) {
                    dir.y = dir.y - CharacterMovement.Instance.ConstantVelocity.y;
                }
            } else if(dir.y < 0) {

                if (CharacterMovement.Instance.ConstantVelocity.y > dir.y) {
                    //dir.y = CharacterMovement.Instance.ConstantVelocity.y - dir.y;
                }
            }

            dir.y /= 2;

            print(dir);
            
            CharacterMovement.Instance.AddImpulse(dir);

            CharacterMovement.Instance.ReleaseGravity = true;

        }



    }

    public void CreateLine(Vector3 End) {

        TempHitPoint = End;
        GameObject obj = Instantiate(ChainLinePrefab);
        LineRenderer renderer = obj.GetComponent<LineRenderer>();

        renderer.SetPosition(0, WeaponTip.position);

        this.ZeStationaryGrappler.SetActive(false);
        this.moveGraplerer.transform.position = End;
        this.moveGraplerer.transform.rotation = Quaternion.Euler(Target_Hit.normal);
        this.moveGraplerer.SetActive(true);
        renderer.SetPosition(1, HookEnd.position);

        CanShoot = false;
        CurrenChainLine = obj;
        StartCoroutine(StopLine());
    }

    public IEnumerator StopLine() {
        yield return new WaitForSeconds(LineDecay);
        Destroy(CurrenChainLine);
        CanShoot = true;

        this.moveGraplerer.SetActive(false);
        this.ZeStationaryGrappler.SetActive(true);

        CharacterMovement.Instance.ReleaseGravity = false;

    }

    public void LateUpdate() {
        if (!CanShoot) {
            LineRenderer renderer = CurrenChainLine.GetComponent<LineRenderer>();

            renderer.SetPosition(0, WeaponTip.position);
            this.moveGraplerer.transform.position = (Target == null ? TempHitPoint : Target.transform.position + TempHitPointOffset);
            this.moveGraplerer.transform.rotation = Quaternion.Euler(Target_Hit.normal);
            renderer.SetPosition(1, HookEnd.position);

        }
    }
}
