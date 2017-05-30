using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

	public XboxController controller;
	public LayerMask layerMask;

	public static bool isBuilding;

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject trapPrefab;
	public GameObject turretPrefab;

	Transform PlayerCameraLookAt;

	void Start() {
		PlayerCameraLookAt = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).transform;
	}

	public void InstantiateObject(GameObject prefab) {
		//instantiate object
		if(isBuilding) {
			GameObject GO = Instantiate (prefab, Vector3.zero, Quaternion.identity);
			Debug.Log (GO.name);
			Ray ray = new Ray(PlayerCameraLookAt.position, PlayerCameraLookAt.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10, layerMask)) {
				GO.transform.position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
			}
		}
	}

	public void BuildObject(int menuOption) {
		switch (menuOption) {
		case 1:
			//open trap menu
			isBuilding = true;
			InstantiateObject(trapPrefab);
			break;
		case 2:
			//open wall menu
			isBuilding = true;
			InstantiateObject(wallPrefab);
			break;
		case 3:
			//open floor menu
			isBuilding = true;
			InstantiateObject(floorPrefab);
			break;
		case 4:
			//open turret menu
			isBuilding = true;
			InstantiateObject(turretPrefab);
			break;
		}
	}
}
