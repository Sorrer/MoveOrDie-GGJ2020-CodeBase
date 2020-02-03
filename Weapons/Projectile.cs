using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float Velocity;
    public float damage;
    public float radius;

    public float PushPower;

    public LayerMask mask;

    public bool UseDeathDrive;

    // Update is called once per frame
    void Update()
    {

        Vector3 LastPosition = this.transform.position;

        this.transform.position += Velocity * Time.deltaTime * this.transform.forward;

        Ray ray = new Ray();
        ray.direction = (LastPosition - this.transform.position).normalized;
        ray.origin = LastPosition;
        RaycastHit hit;
        if (Physics.SphereCast(ray, radius, out hit, (LastPosition - this.transform.position).magnitude, mask.value)){

            Entity e = hit.collider.gameObject.GetComponent<Entity>();

            if (e != null) {
                e.Damage(damage * (UseDeathDrive? GlobalVars.Instance.GetDeathDrivePercentage() : 1));
            }

            Rigidbody body = hit.collider.GetComponent<Rigidbody>();
            if (body != null) {
                body.AddForce((this.transform.position - body.gameObject.transform.position).normalized * PushPower);
            }

            this.transform.position = hit.point;
            Destroy(this.gameObject);


        }


    }


    private void OnTriggerEnter(Collider collision) {

        Entity e = collision.gameObject.GetComponent<Entity>();

        if(e != null) {
            e.Damage(damage);
        }

        Destroy(this.gameObject);
    }
}
