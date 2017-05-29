using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int damage;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			other.GetComponent<Enemy> ().TakeDamage (damage);
			Destroy (this.gameObject);
		}
	}
}
