using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGrunt : Enemy {

	void Start() {
		target = GameObject.FindGameObjectWithTag ("Player1").transform;
		StartCoroutine (UpdatePath ());
	}
}
