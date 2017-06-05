using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienGrunt : Enemy {

	Transform target;

	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player1").transform;
	}

	void Update () {
		agent.SetDestination (target.position);
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2") {
			other.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
			Destroy (gameObject);
		}
	}
}
