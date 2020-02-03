using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalVars : MonoBehaviour
{

    public static GlobalVars Instance;


    [Header("Movement")]
    public AnimationCurve AccelerationCurve;
    public float MaxAceleration;
    public AnimationCurve StoppingCurve;
    public float MaxStopping;

    public AnimationCurve DeathDriveSpeedCurve;
    public float DeathDrive;
    public float MaxDeathDrive;

    public float MinSpeed;
    public float CurrentSpeed;
    public float MaxSpeed;

    public float gravity;
    [Space(4)]
    public DialUI SpeedOMeter;
    public DialUI DeathDriveMeter;
    public GameObject PlayerObj;

    public int EnemiesRemaining = -99;

    public void AddEnemyRemaining() {
        if(EnemiesRemaining == -99) {
            EnemiesRemaining = 1;
        } else {
            EnemiesRemaining++;
        }
    }

    public void RemoveEnemyRemaining() {
        EnemiesRemaining--;
    }

    // Start is called before the first frame update

    public void AddDeathDrive(float amount) {
        DeathDrive += amount;

        DeathDrive = Mathf.Clamp(DeathDrive, 0, MaxDeathDrive);
    }



    public void AddSpeed() {
        CurrentSpeed += AccelerationCurve.Evaluate(GetDeathDrivePercentage()) * Time.deltaTime * MaxAceleration;
        if (CurrentSpeed > GetMaxSpeed()) {
            CurrentSpeed = GetMaxSpeed();
        }

        if(CurrentSpeed < MinSpeed) {
            CurrentSpeed = MinSpeed;
        }

    }
    public void DecreaseSpeed() {
        CurrentSpeed -= StoppingCurve.Evaluate(GetSpeedPercentage()) * Time.deltaTime * MaxStopping;

        if(CurrentSpeed < 0) {
            CurrentSpeed = 0;
        }

    }
    public void DecreaseSpeed(float multiplier) {
        CurrentSpeed -= StoppingCurve.Evaluate(GetSpeedPercentage()) * Time.deltaTime * MaxStopping * multiplier;

        if (CurrentSpeed < 0) {
            CurrentSpeed = 0;
        }

    }


    public float GetMaxSpeed() {

        if(GetDeathDrivePercentage() < 0.1) {
            return this.MinSpeed;
        }

        return MaxSpeed * DeathDriveSpeedCurve.Evaluate(GetDeathDrivePercentage());
    }
    
    public float GetSpeedPercentage() {
        return CurrentSpeed / MaxSpeed;
    }

    public float GetDeathDrivePercentage() {
        return DeathDrive / MaxDeathDrive;
    }

    

    void Start()
    {
        if(Instance != null) {
            Destroy(Instance.gameObject);
            Instance = this;
        } else {
            Instance = this;
        }

        LastPosition = PlayerObj.transform.position;
    }

    Vector3 LastPosition = new Vector3();

    // Update is called once per frame
    void FixedUpdate()
    {


        
        SpeedOMeter.CurrentProgress = Mathf.Lerp(SpeedOMeter.CurrentProgress,CharacterMovement.VelocityMagnitude, 3)/MaxSpeed;
        //print(((PlayerObj.transform.position - LastPosition).magnitude / Time.fixedDeltaTime));
        LastPosition = PlayerObj.transform.position;


        

        DeathDriveMeter.CurrentProgress = GetDeathDrivePercentage();
    }


    public bool ThisIsTheWinningLevel;

    private void Update() {

        if (!ThisIsTheWinningLevel) {
            return;
        }

        if(!DOTHeThing && EnemiesRemaining <= 0 && EnemiesRemaining != -99) {
            WinScreen();


        }

    }

    public GameObject WinPanel;
    public bool DOTHeThing = false;
    private void WinScreen() {
        DOTHeThing = true;
        CharacterMovement.Instance.enabled = false;
        GlobalPlayer.Instance.enabled = false;
        WinPanel.SetActive(true);

        StartCoroutine(WaitForReset());
    }


    private IEnumerator WaitForReset() {
        GlobalSounds.Instance.PlayVictorySong();
        yield return new WaitForSeconds(1f);

        while (!Input.anyKeyDown) {
            yield return null;
        }

        SceneManager.LoadScene(0);


    }
}
