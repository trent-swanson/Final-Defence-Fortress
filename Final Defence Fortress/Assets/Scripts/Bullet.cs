using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float bulletSpeed = 1;

	void Start() {
		Destroy (gameObject, 1);
	}

	void FixedUpdate() {
		transform.position += transform.forward * bulletSpeed;
	}

	void OnCollisionEnter() {
		Destroy (gameObject);
	}
}
