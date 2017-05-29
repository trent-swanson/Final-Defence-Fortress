using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	public int health;
	public float speed;
	public int attackDamage;
	public int killValue;

	GameObject player;
	NavMeshAgent agent;
	public bool chasePlayer;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = speed;
	}

	void Update() {
		if(chasePlayer) {
			agent.destination = player.transform.position;
		}
	}

	public void TakeDamage(int damage) {
		health -= damage;
		if (health > 0) {
			Debug.Log ("Hit");
		} else if (health <= 0) {
			Destroy (gameObject);
		}
	}

}
