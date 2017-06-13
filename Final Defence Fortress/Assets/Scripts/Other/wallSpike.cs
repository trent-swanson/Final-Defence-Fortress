using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallSpike : MonoBehaviour {

	//reference to spikes object
	public GameObject spikes;
	//max timer amount
	public float maxTimer = 10;
	//current timer
	float timer;

	//reference to animator component
	Animator anim;
	//check if we can fire spikes
	bool canFire = true;

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
		timer = maxTimer;
		anim = spikes.GetComponent<Animator> ();
	}

	//--------------------------------------------------------------------------------------
	//	Start()
	// Runs every frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update() {
		if (!canFire) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				canFire = true;
				timer = maxTimer;
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
	void FireSpikes(Collider other) {
		if (other.tag == "Enemy" && canFire) {
			canFire = false;
			StartCoroutine (FireSpikes ());
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerEnter()
	// Coroutine: active gameobject, wait, play animtion, wait, play animation, wait, deativate object
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	IEnumerator FireSpikes() {
		spikes.SetActive (true);
		anim.SetTrigger (Animator.StringToHash("FireWallSpike"));
		yield return new WaitForSeconds (2);
		anim.SetTrigger (Animator.StringToHash("EndWallSpike"));
		yield return new WaitForSeconds (1);
		spikes.SetActive (false);
	}
}
