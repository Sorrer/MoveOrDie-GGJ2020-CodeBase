using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public static MainMenu Instance;
    public GameObject Panel;

    // Start is called before the first frame update
    void Start()
    {
       if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    private void OnLevelWasLoaded(int level) {
        if(level != 0) {
            print("Yeeter");
            return;
        }

    }


    public void RevealSettings() {
        SoundSettings.Instance.Reveal();
    }

    public void TransferToGame() {
        SoundSettings.Instance.TransferToPause();
    }

    public void Hide() {
        Panel.SetActive(false);
    }

    public void Reveal() {
        Panel.SetActive(true);
        
    }
}
