using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

	public HealthManager healthManager = new HealthManager();
	public float Weight;

	public virtual void Damage(float amount) {
		healthManager.Damage(amount);
	}

	public abstract void Pull(Vector3 Force);
}

[System.Serializable]
public class HealthManager {

	public float Health;


	public void Damage(float amount) {
		Health -= amount;
	}
	
	public bool IsDead() {
		if(Health <= 0) {
			return true;
		}

		return false;
	}

}