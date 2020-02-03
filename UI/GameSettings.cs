using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    public static GameSettings Instance;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
