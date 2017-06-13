using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpike : MonoBehaviour {

	//refernce to spikes object
	public GameObject spikes;
	//saved max timer amount
	public float maxTimer = 10;
	//current timer amount
	float timer;

	//Reference to animatior component
	Animator anim;
	//bool to check if we can fire spikes
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
	//	Update()
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
	//	OnCollisionEnter()
	// Runs when collider enters trigger collider
	//
	// Param:
	//		other: reference to collider object that entered trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy" && canFire) {
			canFire = false;
			StartCoroutine (FireSpikes ());
		}
	}

	//--------------------------------------------------------------------------------------
	//	FireSpikes()
	// Coroutine: ativate spikes and play animation, wait, play another animation, wait, and deativate
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	IEnumerator FireSpikes() {
		spikes.SetActive (true);
		anim.SetTrigger (Animator.StringToHash("FireSpikes"));
		yield return new WaitForSeconds (2);
		anim.SetTrigger (Animator.StringToHash("EndSpikes"));
		yield return new WaitForSeconds (1);
		spikes.SetActive (false);
	}

}
