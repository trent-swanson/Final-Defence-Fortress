using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

	public int healthIncrease = 250;
	bool isActive = true;
	public float timer = 5;
	float tempTimer;

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
