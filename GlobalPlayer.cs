using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayer : MonoBehaviour
{

    public static GlobalPlayer Instance;
   

    public Transform Camera;
    public Weapon weapon;
    public Weapon SeconaryWeapon;

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

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.Instance.IsPaused) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {

            if(weapon != null) {
                if (weapon.DeathDriveCost/2 < GlobalVars.Instance.DeathDrive && weapon.Attack()) {
                 GlobalVars.Instance.AddDeathDrive(-weapon.DeathDriveCost);
                }
            }


        }

        if (Input.GetMouseButtonDown(1)) {
            if (SeconaryWeapon.DeathDriveCost / 2 < GlobalVars.Instance.DeathDrive && SeconaryWeapon.Attack()) {
                GlobalVars.Instance.AddDeathDrive(-SeconaryWeapon.DeathDriveCost);
            }
        }

    }
}
