using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildingManager : MonoBehaviour {

	public XboxController controller;
	public LayerMask layerMask;

	public static bool isBuilding;

	public GameObject woodFloor;

	Transform PlayerCameraLookAt;

	void Start() {
		PlayerCameraLookAt = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).transform;
	}

	void Update() {
		if((XCI.GetAxis(XboxAxis.LeftTrigger) > 0) && !isBuilding) {
			GameObject GO = Instantiate (woodFloor, Vector3.zero, Quaternion.identity);
			isBuilding = true;
			Ray ray = new Ray(PlayerCameraLookAt.position, PlayerCameraLookAt.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10, layerMask)) {
				GO.transform.position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
			}
		}
	}
}
