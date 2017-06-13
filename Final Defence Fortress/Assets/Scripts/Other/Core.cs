using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour {

	//max health value
	public int maxHealth = 500;
	//current health value
	public int health;
	//reference to health slider
	public Slider coreHealthSlider;

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
		health = maxHealth;
		coreHealthSlider.value = health / maxHealth;
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
		if (health <= 0) {
			Debug.Log ("Core Destroyed, end game");
			Cursor.visible = true;
			SceneManager.LoadScene ("MainMenu");
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerEnter()
	// Runs when a collider enters trigger
	//
	// Param:
	//		other - reference to the collider that entered trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "Enemy") {
			health -= 60;
			coreHealthSlider.value = (float)health / (float)maxHealth;
			Destroy (other.gameObject);
		}
	}

}
