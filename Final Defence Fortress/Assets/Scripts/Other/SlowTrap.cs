using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowTrap : MonoBehaviour {

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
		if (other.tag == "Enemy") {
			other.GetComponent<NavMeshAgent> ().speed -= 2;
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerExit()
	// Runs when collider exits trigger collider
	//
	// Param:
	//		other: reference to collider object that exited trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerExit(Collider other) {
		if (other.tag == "Enemy") {
			other.GetComponent<NavMeshAgent> ().speed += 2;
		}
	}

}
