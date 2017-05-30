using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

	//a LayerMask object
	public LayerMask layerMask;

	//references to building set objects
	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject trapPrefab;
	public GameObject turretPrefab;

	//refrences to PlayerLookAt transforms
	Transform PlayerLookAt1;
	Transform PlayerLookAt2;

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
		PlayerLookAt1 = GameObject.FindGameObjectWithTag ("Player1").transform.GetChild (0).GetChild(0).GetChild(0).transform.parent.parent.parent;
		PlayerLookAt2 = GameObject.FindGameObjectWithTag ("Player2").transform.GetChild (0).GetChild(0).GetChild(0).transform.parent.parent.parent;
	}

	//--------------------------------------------------------------------------------------
	//	InstantiateObject()
	// Check which player called function, spawn prefab object at location of raycast hit
	// from player that called function
	//
	// Param:
	//		GameObject prefab - prefab to instantiate, int playerNumber - which player called function		
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void InstantiateObject(GameObject prefab, int playerNumber) {
		//instantiate object
		if (playerNumber == 1) {
			GameObject GO = Instantiate (prefab, Vector3.zero, Quaternion.identity);
			GO.transform.SetParent (transform);
			GO.GetComponent<BuildObject> ().playerNumber = playerNumber;
			Ray ray = new Ray(PlayerLookAt1.position, PlayerLookAt1.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 50, layerMask)) {
				GO.transform.position = new Vector3 (hit.point.x, hit.point.y + (GO.GetComponent<BoxCollider>().bounds.size.y / 2), hit.point.z);
			}
		} else if (playerNumber == 2) {
			GameObject GO = Instantiate (prefab, Vector3.zero, Quaternion.identity);
			GO.transform.SetParent (transform);
			GO.GetComponent<BuildObject> ().playerNumber = playerNumber;
			Ray ray = new Ray(PlayerLookAt2.position, PlayerLookAt2.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 50, layerMask)) {
				GO.transform.position = new Vector3 (hit.point.x, hit.point.y + (GO.GetComponent<BoxCollider>().bounds.size.y / 2), hit.point.z);
			}
		}

	}

	//--------------------------------------------------------------------------------------
	//	BuildObject()
	// Call InstantiateObject() passing a building set object decided by menuOption
	//
	// Param:
	//		int menuOption - menu option selected, int playerNumber - index of player that called function	
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void BuildObject(int menuOption, int playerNumber) {

		switch (menuOption) {
		case 1:
			//open trap menu
			//InstantiateObject(trapPrefab, controller);
			break;
		case 2:
			//open wall menu
			InstantiateObject(wallPrefab, playerNumber);
			break;
		case 3:
			//open floor menu
			InstantiateObject(floorPrefab, playerNumber);
			break;
		case 4:
			//open turret menu
			//InstantiateObject(turretPrefab, controller);
			break;
		}
	}
}
