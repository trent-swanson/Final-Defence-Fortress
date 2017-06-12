using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player1") {
			other.GetComponent<PlayerController> ().moveSpeed -= 8;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player1") {
			other.GetComponent<PlayerController> ().moveSpeed += 8;
		}
	}

}
