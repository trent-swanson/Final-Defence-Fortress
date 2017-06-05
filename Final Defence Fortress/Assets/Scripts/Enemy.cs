using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	protected NavMeshAgent agent;

	public int health = 100;
	public int damage = 50;

	public void TakeDamage() {
		health -= 25;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

}
