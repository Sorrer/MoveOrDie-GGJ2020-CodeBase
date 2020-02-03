using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance;


    public GameObject Panel;
    public bool IsTheActive;



    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level) {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (SoundSettings.Instance.InPauseMenu) {

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {

                if (IsTheActive) {
                    Hide();
                } else {
                    Reveal();
                }

            }


        }
    }


    public bool IsPaused = false;
    

    public void Hide() {
        if (CharacterMovement.Instance != null) {
            CharacterMovement.Instance.enabled = true;
        }
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsTheActive = false;
        Panel.SetActive(false);
        SoundSettings.Instance.Hide();
    }

    public void TempHide() {
        Panel.SetActive(false);
    }

    public void Reveal() {
        IsPaused = true;
        if (CharacterMovement.Instance != null) {
            CharacterMovement.Instance.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsTheActive = true;
        Panel.SetActive(true);
    }
}
