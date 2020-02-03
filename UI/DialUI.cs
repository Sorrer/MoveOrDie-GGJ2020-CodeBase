using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialUI : MonoBehaviour
{
    [Range(0, 1)]
    public float CurrentProgress;
    public RectTransform dial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dial.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-80, 80, 1-CurrentProgress));
    }
}
