using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

	//amount to increase players health by
	public int healthIncrease = 250;
	//check if healthpack is active
	bool isActive = true;
	//timer amount
	public float timer = 5;
	//current timer
	float tempTimer;

	//--------------------------------------------------------------------------------------
	//	Update()
	// Runs every frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update() {
		if (!isActive) {
			tempTimer -= Time.deltaTime;
			if (tempTimer <= 0) {
				isActive = true;
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
				gameObject.GetComponent<BoxCollider> ().enabled = true;
				transform.GetChild (0).gameObject.SetActive (true);
			}
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
		if(other.tag == "Player1" || other.tag == "Player2") {
			if (other.GetComponent<PlayerController> ().playerHealth < other.GetComponent<PlayerController> ().maxPlayerHealth) {
				other.GetComponent<PlayerController> ().AddHealth (healthIncrease);
				tempTimer = timer;
				isActive = false;
				transform.GetChild (0).gameObject.SetActive (false);
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
				gameObject.GetComponent<BoxCollider> ().enabled = false;
			}
		}
	}
}
