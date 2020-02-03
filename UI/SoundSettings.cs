using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{

    public static SoundSettings Instance;

    public bool InPauseMenu; // If not in pause menu in main menu

    public AudioMixer Mixer;

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

        DontDestroyOnLoad(this.gameObject);
    }

    public void Back() {
        if (InPauseMenu) {
            PauseMenu.Instance.Reveal();
            Hide();
        } else {
            MainMenu.Instance.Reveal();
            Hide();
        }
    }

    public void TransferToPause() {
        InPauseMenu = true;
    }
    public void TransferToMainMenu() {
        InPauseMenu = false;
    }

    public void Hide() {
        Panel.SetActive(false);
    }

    public void Reveal() {
        Panel.SetActive(true);
        
    }

    public void SetMasterVolume(Slider slider) {
        Mixer.SetFloat("MasterVolume", Mathf.Log(slider.value) * 20);
    }
    public void SetMusicVolume(Slider slider) {
        Mixer.SetFloat("MusicVolume", Mathf.Log(slider.value) * 20);
    }
    public void SetAmbienceVolume(Slider slider) {
        Mixer.SetFloat("AmbienceVolume", Mathf.Log(slider.value) * 20);
    }
    public void SetSFXVolume(Slider slider) {
        Mixer.SetFloat("AmbienceVolume", Mathf.Log(slider.value) * 20);
    }
}
