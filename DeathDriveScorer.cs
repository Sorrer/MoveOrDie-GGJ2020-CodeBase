using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathDriveScorer : MonoBehaviour
{

    public static DeathDriveScorer Instance;


    public float DashCost;

    
    public AnimationCurve SPEED_DeathScoreCurve;
    public float SPEED_maxDeathScore;
    // Start is called before the first frame update
    public AnimationCurve DeathDriveDecayCurve;
    public float DeathDriveDecay;
    void Start()
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    Coroutine DeathTimerC;

    // Update is called once per frame
    void Update()
    {
        GlobalVars.Instance.AddDeathDrive(SPEED_DeathScoreCurve.Evaluate(CharacterMovement.VelocityMagnitude/GlobalVars.Instance.MaxSpeed)*SPEED_maxDeathScore * Time.deltaTime);

        if(CharacterMovement.VelocityMagnitude < 2f) {
            if(!PauseMenu.Instance.IsPaused) GlobalVars.Instance.AddDeathDrive(DeathDriveDecayCurve.Evaluate(GlobalVars.Instance.GetDeathDrivePercentage()) * DeathDriveDecay * Time.deltaTime);
        }


        if(GlobalVars.Instance.GetDeathDrivePercentage() <= 0.05f) {
            if(DeathTimerC == null) {
                DeathTimerC = StartCoroutine(DeathTimer());
                DeathText.text = "YOU DID NOT MOVE";
            }
        } else {
            if(DeathTimerC != null) {
                MoveOrDieText.gameObject.SetActive(false);
                StopCoroutine(DeathTimerC);
                DeathTimerC = null;
            }
        }

        if(CharacterMovement.Instance.transform.position.y <= -20f) {
            Die();
            DeathText.text = "YOU FELL . . .";
        }

    }

    public float DeathTimerLength;
    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI MoveOrDieText;

    public IEnumerator DeathTimer() {
        MoveOrDieText.gameObject.SetActive(true);
        yield return new WaitForSeconds(DeathTimerLength);
        MoveOrDieText.gameObject.SetActive(false);

        Die();

    }


    public GameObject DeathPanel;

    public void Die() {
        GlobalSounds.Instance.StopMusic();
        GlobalSounds.Instance.PlayDeathSFX();
        CharacterMovement.Instance.enabled = false;
        DeathPanel.SetActive(true);
        StartCoroutine(EndScreenTimer());
    }

    public IEnumerator EndScreenTimer() {
        yield return new WaitForSeconds(1.5f);
        while (!Input.anyKeyDown) {
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public bool CanSpendPoints(float Cost) {
        if(Cost > GlobalVars.Instance.DeathDrive) {
            return false;
        }
        return true;
    }

    public bool SpendPoints(float cost) {
        if (CanSpendPoints(cost)) {
            GlobalVars.Instance.AddDeathDrive(-cost);

            return true;
        } else {
            return false;
        }
    }
}
