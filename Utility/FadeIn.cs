using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeIn : MonoBehaviour {
    public TextMeshProUGUI ui;

    public float TimeToFinish = 5f;
    public float CurrentValue;

    Coroutine currentine;
    public void Show() {
        CurrentValue = 0;
        ui.fontSharedMaterial.SetFloat("_FaceDilate", Mathf.Lerp(-1, 0.8f, CurrentValue/ TimeToFinish));

        currentine = StartCoroutine(DoShow());
    }

    public IEnumerator DoShow(){

        while(true){
            CurrentValue += Time.deltaTime;

            ui.fontSharedMaterial.SetFloat("_FaceDilate", Mathf.Lerp(-1, 0.8f, CurrentValue / TimeToFinish));

            if (CurrentValue/ TimeToFinish > 1) {
                yield break;
            }



            yield return null;
        }

    }

    private void OnEnable() {
        if(currentine != null) {
            StopCoroutine(currentine);
        }
        ui.material.SetFloat("_FaceDilate", -1);
        Show();
    }




}
