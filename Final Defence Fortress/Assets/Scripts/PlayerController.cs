using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour {

	public XboxController controller1;

	public float moveSpeed = 10;
	public float turnSpeed = 10;
	public float jumpHeight = 5;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 6;

	void Update () {
		if (XCI.GetAxis(XboxAxis.LeftStickY, controller1) > 0) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
		if (XCI.GetAxis(XboxAxis.LeftStickY, controller1) < 0) {
			transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
		}
		if (XCI.GetAxis(XboxAxis.LeftStickX, controller1) < 0) {
			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
		}
		if (XCI.GetAxis(XboxAxis.LeftStickX, controller1) > 0) {
			transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller1) < 0) {
			transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller1) > 0) {
			transform.Rotate (Vector3.up * turnSpeed * Time.deltaTime);
		}
		if (XCI.GetButtonDown(XboxButton.A, controller1)) {
			GetComponent<Rigidbody> ().AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
		}
		if (XCI.GetAxis(XboxAxis.RightTrigger, controller1) > 0) {
			GameObject tempBullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
			tempBullet.GetComponent<Rigidbody>().velocity = tempBullet.transform.forward * bulletSpeed;
		}
	}
}
