using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

	public float DeathDriveCost;

	public bool Attack() {
		return OnAttack();
	}

	public abstract bool OnAttack();

}	
