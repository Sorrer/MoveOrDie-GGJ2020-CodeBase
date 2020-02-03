using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSounds : MonoBehaviour
{

    public static GlobalSounds Instance;

    public AudioSource DeathSFX;
    public AudioSource FeetSource;
    public AudioSource HeadSource;
    public AudioSource MusicSource;


    public List<AudioClip> PistolShoots = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(Instance.gameObject);
            Instance = this;
        }



    }

    public void StopMusic() {
        MusicSource.Stop();
    }

    public void PlayDeathSFX() {
        DeathSFX.PlayOneShot(DeathSFX.clip);
        
    }

    public void PlayPistolSFX(AudioSource source) {

        source.PlayOneShot(PistolShoots[Mathf.RoundToInt(Random.value * (PistolShoots.Count - 1))]);

    }

    public float TimeBetweenFootSteps;
    public float MinTimeBetweenFootSteps;
    public List<AudioClip> FootSteps = new List<AudioClip>();
    private Coroutine FootStepsUpdate;

    public void PlayFootsteps() {
        if(FootStepsUpdate != null) {
            return;
        }

        FootStepsUpdate = StartCoroutine(FootStepsUpdater());
    }

    public IEnumerator FootStepsUpdater() {
        while (true) {

            FeetSource.pitch = Random.Range(0.6f, 1f);
            FeetSource.volume = Random.Range(0.3f, 0.5f);
            FeetSource.PlayOneShot(FootSteps[Mathf.RoundToInt(Random.value * (FootSteps.Count - 1))]);
            yield return new WaitForSeconds(MinTimeBetweenFootSteps + (TimeBetweenFootSteps * (1 - GlobalVars.Instance.GetSpeedPercentage())));

        }
    }


    public void StopFootsteps() {
        if (FootStepsUpdate == null) return;

        StopCoroutine(FootStepsUpdate);
        FootStepsUpdate = null;
    }


    public List<AudioClip> JumpSounds;

    public void PlayJumpSound() {
        HeadSource.PlayOneShot(JumpSounds[Mathf.RoundToInt(Random.value * (JumpSounds.Count - 1))]);
    }

    public AudioClip VictorySong;

    public void PlayVictorySong() {
        StopMusic();
        HeadSource.PlayOneShot(VictorySong);
        HeadSource.loop = true;
    }

}
