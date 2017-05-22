using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;
	public XboxController controller;

	//moving
	public float moveSpeed = 60;
	public float jumpHeight = 5;
	public float turnSpeed = 10;
	public float lookUpSpeed = 10;
	bool isGrounded;

	//shooting
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 6;
	float shootingTimerRight;
	float shootingTimerLeft;
	public float timeBetweenShots = 0.02f;

	public Vector3 rotationHolder = Vector3.zero;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		rotationHolder = transform.GetChild (0).rotation.eulerAngles;
	}

	void Update () {
		//move player
		float axisX = XCI.GetAxis (XboxAxis.LeftStickX, controller);
		float axisZ = XCI.GetAxis (XboxAxis.LeftStickY, controller);

		Vector3 movement = new Vector3 (axisX, 0, axisZ);
		transform.Translate (movement * moveSpeed * Time.deltaTime);

		//rotate player
		float rotateAxisY = XCI.GetAxis (XboxAxis.RightStickX, controller);
		float rotateAxisX = XCI.GetAxis (XboxAxis.RightStickY, controller);

		Vector3 turnVector = new Vector3 (0, rotateAxisY, 0);
		transform.Rotate (turnVector * turnSpeed * Time.deltaTime);

		//look up and down
		Vector3 lookVector = new Vector3 (-rotateAxisX, 0, 0);
		rotationHolder += lookVector;
		rotationHolder.x = Mathf.Clamp (rotationHolder.x, -20, 20);
		transform.GetChild (0).transform.localRotation = Quaternion.Euler(rotationHolder.x, transform.rotation.y, 0);

		//jump
		CheckGrounded();
		if (XCI.GetButtonDown(XboxButton.A, controller) && isGrounded == true) {
			rb.AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
			isGrounded = false;
		}

		//shoot
		FireGun ();
	}

	//return true if raycast hit ground
	void CheckGrounded() {
		float rayLength = 1f;
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
		// if there is something directly below the player
		if (Physics.Raycast(ray, out hit, rayLength)) {
			isGrounded = true;
		}
	}

	void FireGun() {
		if (XCI.GetAxis (XboxAxis.RightTrigger, controller) > 0.1f) {
			if (Time.time - shootingTimerRight > timeBetweenShots) {
				GameObject GO = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
				GO.GetComponent<Rigidbody> ().AddForce (bulletSpawn.transform.forward * bulletSpeed, ForceMode.Impulse);
				Destroy (GO, 3);
				shootingTimerRight = Time.time;
			}
		}
	}
}
