using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			transform.parent.GetComponent<Enemy>().chasePlayer = true;
		}
	}
}
