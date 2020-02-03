using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : Weapon
{

    public List<GameObject> lines = new List<GameObject>();

    public float baseDamage;
    public float damage;
    public float MaxRange;
    public Transform WeaponTip;
    public GameObject ShootingLinePrefab;
    public float LineDecay;
    public float PushPower;

    public AudioSource audio;

    bool CanShoot = true;

    public override bool OnAttack() {
        if (CanShoot) {
            RaycastHit hit;
            if (Physics.Raycast(GlobalPlayer.Instance.Camera.position, GlobalPlayer.Instance.Camera.transform.forward, out hit, MaxRange)) {
                CreateLine(hit.point);

                Entity e = hit.collider.gameObject.GetComponent<Entity>();
                if (e != null) {
                    e.Damage(baseDamage + (damage * GlobalVars.Instance.GetDeathDrivePercentage()));
                }

                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if(body != null) {
                    body.AddForce((hit.point- body.gameObject.transform.position).normalized * PushPower);
                }


            } else {
                CreateLine(GlobalPlayer.Instance.Camera.position + GlobalPlayer.Instance.Camera.forward * MaxRange);
            }
            return true;
        }
        return false;


    }

    public void CreateLine(Vector3 End) {

        GlobalSounds.Instance.PlayPistolSFX(audio);

        GameObject obj = Instantiate(ShootingLinePrefab);
        LineRenderer renderer = obj.GetComponent<LineRenderer>();

        renderer.SetPosition(0, WeaponTip.position);
        renderer.SetPosition(1, End);

        lines.Add(obj);
        StartCoroutine(DeleteTimer(LineDecay, obj));
    }

    public IEnumerator DeleteTimer(float time, GameObject removeMe) {
        yield return new WaitForSeconds(LineDecay);

        lines.Remove(removeMe);
        Destroy(removeMe);
    }


}
