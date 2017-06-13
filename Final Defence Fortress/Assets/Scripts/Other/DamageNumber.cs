using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour {

	//rference to text mesh object
	public TextMesh textMesh;
	//saved colour for low, normal, and high damage amounts
	public Color highDamageColor;
	public Color normalDamageColor;
	public Color lowDamageColor;

	//played index
	int playerID;
	//transform of player1's camera
	Transform cameraOne;
	//transform of player2's camera
	Transform cameraTwo;

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
		cameraOne = GameObject.FindGameObjectWithTag ("CameraOne").transform;
		cameraTwo = GameObject.FindGameObjectWithTag ("CameraTwo").transform;
		if (playerID == 1) {
			transform.LookAt (cameraOne);
		} else if (playerID == 2) {
			transform.LookAt (cameraTwo);
		}
		Destroy (gameObject, 1);
	}

	//--------------------------------------------------------------------------------------
	//	Start()
	// Change text to damage amount and change text colour
	//
	// Param:
	//		damage - number to change text to, playerNumber which player called function
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void Initialise(int damage, int playerNumber) {
		playerID = playerNumber;
		textMesh.text = damage.ToString ();
		if (damage <= 21) {
			textMesh.color = lowDamageColor;
		} else if (damage <= 32) {
			textMesh.color = normalDamageColor;
		} else if (damage >= 33) {
			textMesh.color = highDamageColor;
		}
	}

}
