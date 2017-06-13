using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//bullet speed
	public float bulletSpeed = 1;

	//--------------------------------------------------------------------------------------
	//	PlaceObject()
	// Runs during initialisation
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Start() {
		Destroy (gameObject, 1);
	}

	//--------------------------------------------------------------------------------------
	//	FixedUpdate()
	// Runs ever fixed framerate frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void FixedUpdate() {
		transform.position += transform.forward * bulletSpeed;
	}

	//--------------------------------------------------------------------------------------
	//	OnCollisionEnter()
	// Runs when object collides whith another collider
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnCollisionEnter() {
		Destroy (gameObject);
	}
}
