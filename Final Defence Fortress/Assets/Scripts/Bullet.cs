using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//bullet damage value
	public int damage;

	//--------------------------------------------------------------------------------------
	//	OnTriggerEnter()
	// If hit enemy, enemy takes daname, destroy this bullet
	//
	// Param:
	//		Collider other - reference to object that entered trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) {
		/*if (other.tag == "Enemy") {
			other.GetComponent<Enemy> ().TakeDamage (damage);
			Destroy (this.gameObject);
		} */
	}
}
