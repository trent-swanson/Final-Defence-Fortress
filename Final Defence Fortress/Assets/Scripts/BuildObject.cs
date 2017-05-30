using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildObject : MonoBehaviour {

	public XboxController controller;
	public LayerMask layerMask;

	public int xSnapStep;
	public int ySnapStep;
	public int zSnapStep;

	public bool isPlaced;
	public bool isSnapped = false;
	Transform PlayerCameraLookAt;

	public Color canPlace;
	public Color cantPlace;

	bool placeTigger = false;

	void Start() {
		PlayerCameraLookAt = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).transform;
	}

	void Update() {
		//while not placed follow mouse position
		if(!isPlaced && !isSnapped) {
			BuildingManager.isBuilding = true;
			float rayLength = 10;
			Ray ray = new Ray(PlayerCameraLookAt.position, PlayerCameraLookAt.forward);
			Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, rayLength, layerMask)) {
				//grid snapping
				Vector3 pos = hit.point;
				int gridSteps = Mathf.RoundToInt (pos.x / xSnapStep);
				pos.x = ((float)gridSteps) * xSnapStep;

				//gridSteps = Mathf.RoundToInt (pos.y / ySnapStep);
				//pos.y = (((float)gridSteps) * ySnapStep) + 0.1f;
				pos.y += 0.1f;

				gridSteps = Mathf.RoundToInt (pos.z / zSnapStep);
				pos.z = ((float)gridSteps) * zSnapStep;
				transform.position = pos;
			}
		}

		//place object
		if(XCI.GetAxisRaw(XboxAxis.RightTrigger) > 0) {
			placeTigger = true;
		}
		if((XCI.GetAxisRaw(XboxAxis.RightTrigger) == 0) && isSnapped && BuildingManager.isBuilding && placeTigger) {
			transform.GetChild (1).gameObject.SetActive (false);
			transform.GetChild (0).gameObject.SetActive (true);
			placeTigger = false;
			isPlaced = true;
			Debug.Log (BuildingManager.isBuilding);
			BuildingManager.isBuilding = false;
		}

		//release snapping
		if (isSnapped && !isPlaced) {
			float rayLength = 10;
			Ray ray = new Ray(PlayerCameraLookAt.position, PlayerCameraLookAt.forward);
			Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, rayLength, layerMask)) {
				//if look position not == to object position un-snap
				if (Vector3.Distance(hit.point,transform.position) > 7f) {
					isSnapped = false;
				}
			}
		}

		//can place colour change
		if (isSnapped) {
			transform.GetChild (1).GetComponent<Renderer> ().material.color = canPlace;
		} else {
			transform.GetChild (1).GetComponent<Renderer> ().material.color = cantPlace;
		}
	}

	public bool V3Equal(Vector3 a, Vector3 b, float campareDistance) {
		return Vector3.SqrMagnitude (a - b) < campareDistance;
	}
}
