using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowTrap : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			other.GetComponent<NavMeshAgent> ().speed -= 2;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Enemy") {
			other.GetComponent<NavMeshAgent> ().speed += 2;
		}
	}

}
