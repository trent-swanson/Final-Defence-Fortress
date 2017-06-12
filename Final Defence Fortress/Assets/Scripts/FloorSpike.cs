using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpike : MonoBehaviour {

	public GameObject spikes;
	public float maxTimer = 10;
	float timer;

	Animator anim;
	bool canFire = true;

	void Start() {
		timer = maxTimer;
		anim = spikes.GetComponent<Animator> ();
	}

	void Update() {
		if (!canFire) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				canFire = true;
				timer = maxTimer;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy" && canFire) {
			canFire = false;
			StartCoroutine (FireSpikes ());
		}
	}

	IEnumerator FireSpikes() {
		spikes.SetActive (true);
		anim.SetTrigger (Animator.StringToHash("FireSpikes"));
		yield return new WaitForSeconds (2);
		anim.SetTrigger (Animator.StringToHash("EndSpikes"));
		yield return new WaitForSeconds (1);
		spikes.SetActive (false);
	}

}
