using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	public int health = 100;
	public int damage = 50;

	public void TakeDamage() {
		health -= 25;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

}
