using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour {

	public TextMesh textMesh;
	public Color highDamageColor;
	public Color normalDamageColor;
	public Color lowDamageColor;

	int playerID;
	Transform cameraOne;
	Transform cameraTwo;

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
