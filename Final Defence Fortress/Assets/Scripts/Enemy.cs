using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	protected NavMeshAgent agent;

	public int health = 100;
	public int damage = 50;

	public void TakeDamage(int p_damage) {
		health -= p_damage;
		Debug.Log (health);
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

}
