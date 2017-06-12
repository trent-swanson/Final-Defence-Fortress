using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	//enum of enemy states
	public enum state {DestroyCore, Idle, ChasePlayer};
	//enemyState is a state object and is set to DestroyCore
	public state enemyState = state.DestroyCore;

	protected NavMeshAgent agent;
	protected Transform coreTarget;
	protected Transform player1;
	protected Transform player2;

	public int health = 100;
	public int damage = 50;
	public int chasePlayerDistance = 30;
	public GameObject damagePrefab;

	public void TakeDamage(int p_damage) {
		health -= p_damage;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		coreTarget = GameObject.FindGameObjectWithTag ("Core").transform;
		player1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		player2 = GameObject.FindGameObjectWithTag ("Player2").transform;
	}

	void Update () {

		CheckPlayerDistance ();

		switch(enemyState) {
		case state.ChasePlayer:
			break;
		case state.DestroyCore:
			agent.SetDestination (coreTarget.position);
			break;
		case state.Idle:
			break;
		}
	}

	void CheckPlayerDistance() {
		if (Vector3.Distance(player1.position, transform.position) < chasePlayerDistance && Vector3.Distance(player2.position, transform.position) < chasePlayerDistance) {
			if (Vector3.Distance(player1.position, transform.position) < Vector3.Distance(player2.position, transform.position)) {
				agent.SetDestination (player1.position);
				enemyState = state.ChasePlayer;
				return;
			} else {
				agent.SetDestination (player2.position);
				enemyState = state.ChasePlayer;
				return;
			}
		} else if (Vector3.Distance(player1.position, transform.position) < chasePlayerDistance) {
			agent.SetDestination (player1.position);
			enemyState = state.ChasePlayer;
			return;
		} else if (Vector3.Distance(player2.position, transform.position) < chasePlayerDistance) {
			agent.SetDestination (player2.position);
			enemyState = state.ChasePlayer;
			return;
		} else {
			enemyState = state.DestroyCore;
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2") {
			other.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Spikes") {
			int playerID;
			if (Vector3.Distance(player1.position, transform.position) < Vector3.Distance(player2.position, transform.position)) {
				playerID = 1;
			} else {
				playerID = 2;
			}
			int damage = Random.Range (20, 40);
			GameObject GO = Instantiate (damagePrefab, transform.position, Quaternion.identity) as GameObject;
			GO.GetComponent<DamageNumber> ().Initialise (damage, playerID);
			TakeDamage(damage);
		}
	}

}
