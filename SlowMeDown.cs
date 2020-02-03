using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMeDown : MonoBehaviour
{

    public CharacterMovement controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<CapsuleCollider>().height = controller.controller.height - 0.25f;
    }


    private void OnTriggerStay(Collider other) {



        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body != null) {
            body.AddForce((body.gameObject.transform.position - this.GetComponent<CapsuleCollider>().ClosestPoint(body.gameObject.transform.position)).normalized * (controller.MinPushPower + (controller.MaxPushPower * GlobalVars.Instance.GetSpeedPercentage())));

        }
    }

}
