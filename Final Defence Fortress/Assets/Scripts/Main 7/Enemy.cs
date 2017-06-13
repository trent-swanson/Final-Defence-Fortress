using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	//enum of enemy states
	public enum state {DestroyCore, Idle, ChasePlayer};
	//enemyState is a state object and is set to DestroyCore
	public state enemyState = state.DestroyCore;

	//reference to NavMeshAgent component
	protected NavMeshAgent agent;
	//trandform of base core
	protected Transform coreTarget;
	//transform of player 1
	protected Transform player1;
	//transform of player 2
	protected Transform player2;

	//enemy health
	public int health = 100;
	//enemy damage
	public int damage = 50;
	//check distance for in range of player
	public int chasePlayerDistance = 30;
	//reference to damage prefab
	public GameObject damagePrefab;

	//--------------------------------------------------------------------------------------
	//	TakeDamage()
	// Take enemy health, Destroy if 0 or below
	//
	// Param:
	//		p_damage: amount to - health by
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void TakeDamage(int p_damage, int player_ID) {
		health -= p_damage;
		if (health <= 0) {
			if (player_ID == 1) {
				player1.GetComponent<PlayerController> ().gold += 15;
			} else if (player_ID == 2) {
				player2.GetComponent<PlayerController> ().gold += 15;
			} else if (player_ID == 3) {
				player1.GetComponent<PlayerController> ().gold += 25;
				player2.GetComponent<PlayerController> ().gold += 25;
			}
			player1.GetComponent<PlayerController> ().UpdateGold ();
			player2.GetComponent<PlayerController> ().UpdateGold ();
			Destroy (gameObject);
		}
	}

	//--------------------------------------------------------------------------------------
	//	Start()
	// Runs during initialisation
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		coreTarget = GameObject.FindGameObjectWithTag ("Core").transform;
		player1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		player2 = GameObject.FindGameObjectWithTag ("Player2").transform;
	}

	//--------------------------------------------------------------------------------------
	//	Update()
	// Runs every frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update () {

		CheckTargetDistance ();

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

	//--------------------------------------------------------------------------------------
	//	CheckTargetDistance()
	// Check distance to core position and players position
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void CheckTargetDistance() {
		if (Vector3.Distance(coreTarget.position, transform.position) < 8) {
			enemyState = state.DestroyCore;
		} else if (Vector3.Distance(player1.position, transform.position) < chasePlayerDistance && Vector3.Distance(player2.position, transform.position) < chasePlayerDistance) {
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

	//--------------------------------------------------------------------------------------
	//	OnCollisionEnter()
	// Runs when colliding with another collider
	//
	// Param:
	//		other: reference to object collided with
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2") {
			other.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
			Destroy (gameObject);
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerEnter()
	// Runs when collider enters trigger collider
	//
	// Param:
	//		other: reference to collider object that entered trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
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
			TakeDamage(damage, 3);
		}
	}

}
