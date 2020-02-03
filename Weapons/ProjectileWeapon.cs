using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

	public Transform Head;

	public GameObject BulletPrefab;
	public float AttackCooldown;

	public bool CanShoot = true;

	public override bool OnAttack() {



		if (CanShoot) {


			GameObject obj = Instantiate(BulletPrefab);

			obj.transform.rotation = Head.rotation;
			obj.transform.position = this.Head.transform.position + Head.forward * 2f;
			StartCoroutine(Cooldown());
			


			
			CanShoot = false;

			return true;


		}

		return false;
	}

	public IEnumerator Cooldown() {

		yield return new WaitForSeconds(AttackCooldown);
		CanShoot = true;



	}
}
