using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

	//a LayerMask object
	public LayerMask layerMask;

	//reference to item UI panels
	[Space(20)]
	[Header("UI Stuff")]
	public GameObject floorMenu1;
	public GameObject wallMenu1;
	public GameObject floorMenu2;
	public GameObject wallMenu2;
	public Color normalColour;
	public Color highlightColour;

	//references to building set objects
	[Space(20)]
	[Header("Building Prefabs")]
	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject stairPrefab;
	public GameObject slowTrapPrefab;
	public GameObject floorSpikePrefab;
	public GameObject wallSpikePrefab;
	public GameObject healthPack;

	//refrences to PlayerLookAt transforms
	PlayerController player1;
	PlayerController player2;

	int player1WallIndex = 1;
	int player1FloorIndex = 1;

	int player2WallIndex = 1;
	int player2FloorIndex = 1;

	GameObject tempReference1;
	GameObject tempReference2;

	//--------------------------------------------------------------------------------------
	//	OnEnable()
	// Runs when eneabled
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnEnable()
	{
		PlayerController.onExit += CloseItemMenu;
		BuildObject.onCloseItemMenu += CloseItemMenu;
	}

	//--------------------------------------------------------------------------------------
	//	OnDisable()
	// Runs when disabled
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnDisable()
	{
		PlayerController.onExit -= CloseItemMenu;
		BuildObject.onCloseItemMenu -= CloseItemMenu;
	}

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
		player1 = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController>();
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController>();
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
			tempReference1 = GO;
			GO.transform.SetParent (transform);
			GO.GetComponent<BuildObject> ().playerNumber = playerNumber;
			Ray ray = new Ray(player1.crosshair.position, player1.aimDirection);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 50, layerMask)) {
				GO.transform.position = new Vector3 (hit.point.x, hit.point.y + (GO.GetComponent<BoxCollider>().bounds.size.y / 2), hit.point.z);
			}
		} else if (playerNumber == 2) {
			GameObject GO = Instantiate (prefab, Vector3.zero, Quaternion.identity);
			tempReference2 = GO;
			GO.transform.SetParent (transform);
			GO.GetComponent<BuildObject> ().playerNumber = playerNumber;
			Ray ray = new Ray(player2.crosshair.position, player2.aimDirection);
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
	public void OnBuildObject(int menuOption, int playerNumber) {

		switch (menuOption) {
		case 1:
			//open health pack menu
			InstantiateObject(healthPack, playerNumber);
			break;
		case 2:
			//open wall menu
			player1WallIndex = 1;
			player2WallIndex = 1;
			if (playerNumber == 1) {
				wallMenu1.SetActive (true);
			} else if (playerNumber == 2) {
				wallMenu2.SetActive (true);
			}
			UpdateWallMenu(playerNumber);
			break;
		case 3:
			//open floor menu
			player1FloorIndex = 1;
			player2FloorIndex = 1;
			if (playerNumber == 1) {
				floorMenu1.SetActive (true);
			} else if (playerNumber == 2) {
				floorMenu2.SetActive (true);
			}
			UpdateFloorMenu(playerNumber);
			break;
		case 4:
			//open stair menu
			InstantiateObject(stairPrefab, playerNumber);
			break;
		}
	}

	void UpdateWallMenu(int playerNumber) {
		if (playerNumber == 1) {
			if (player1WallIndex == 1) {
				wallMenu1.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				wallMenu1.transform.GetChild (0).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(wallPrefab, playerNumber);
			}
			if (player1WallIndex == 2) {
				wallMenu1.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				wallMenu1.transform.GetChild (1).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(wallSpikePrefab, playerNumber);
			}
		}
		if (playerNumber == 2) {
			if (player2WallIndex == 1) {
				wallMenu2.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				wallMenu2.transform.GetChild (0).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(wallPrefab, playerNumber);
			}
			if (player2WallIndex == 2) {
				wallMenu2.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				wallMenu2.transform.GetChild (1).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(wallSpikePrefab, playerNumber);
			}
		}
	}

	void UpdateFloorMenu(int playerNumber) {
		if (playerNumber == 1) {
			if (player1FloorIndex == 1) {
				floorMenu1.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (2).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (0).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(floorPrefab, playerNumber);
			}
			if (player1FloorIndex == 2) {
				floorMenu1.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (2).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (1).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(slowTrapPrefab, playerNumber);
			}
			if (player1FloorIndex == 3) {
				floorMenu1.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				floorMenu1.transform.GetChild (2).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(floorSpikePrefab, playerNumber);
			}
		}
		if (playerNumber == 2) {
			if (player2FloorIndex == 1) {
				floorMenu2.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (2).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (0).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(floorPrefab, playerNumber);
			}
			if (player2FloorIndex == 2) {
				floorMenu2.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (2).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (1).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(slowTrapPrefab, playerNumber);
			}
			if (player2FloorIndex == 3) {
				floorMenu2.transform.GetChild (0).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (1).GetComponent<Image> ().color = normalColour;
				floorMenu2.transform.GetChild (2).GetComponent<Image> ().color = highlightColour;
				InstantiateObject(floorSpikePrefab, playerNumber);
			}
		}
	}

	void Update() {
		//player 1 left and right D pad
		if (XCI.GetButtonDown(XboxButton.DPadLeft, player1.controller) && player1.isBuilding) {
			player1WallIndex--;
			player1FloorIndex--;
			if (player1WallIndex < 1) {
				player1WallIndex = 2;
			}
			if (player1FloorIndex < 1) {
				player1FloorIndex = 3;
			}
			Destroy (tempReference1);
			if (wallMenu1.activeSelf == true || wallMenu2.activeSelf == true) {
				UpdateWallMenu (1);
			}
			if (floorMenu1.activeSelf == true || floorMenu2.activeSelf == true) {
				UpdateFloorMenu (1);
			}
		}
		if (XCI.GetButtonDown(XboxButton.DPadRight, player1.controller) && player1.isBuilding) {
			player1WallIndex++;
			player1FloorIndex++;
			if (player1WallIndex > 2) {
				player1WallIndex = 1;
			}
			if (player1FloorIndex > 3) {
				player1FloorIndex = 1;
			}
			Destroy (tempReference1);
			if (wallMenu1.activeSelf == true || wallMenu2.activeSelf == true) {
				UpdateWallMenu (1);
			}
			if (floorMenu1.activeSelf == true || floorMenu2.activeSelf == true) {
				UpdateFloorMenu (1);
			}
		}

		//player 2 left and right D pad
		if (XCI.GetButtonDown(XboxButton.DPadLeft, player2.controller) && player2.isBuilding) {
			player1WallIndex--;
			player1FloorIndex--;
			if (player1WallIndex < 1) {
				player1WallIndex = 2;
			}
			if (player1FloorIndex < 1) {
				player1WallIndex = 3;
			}
			Destroy (tempReference2);
			if (wallMenu1.activeSelf == true || wallMenu2.activeSelf == true) {
				UpdateWallMenu (2);
			}
			if (floorMenu1.activeSelf == true || floorMenu2.activeSelf == true) {
				UpdateFloorMenu (2);
			}
		}
		if (XCI.GetButtonDown(XboxButton.DPadRight, player2.controller) && player2.isBuilding) {
			player2WallIndex++;
			player2FloorIndex++;
			if (player2WallIndex > 2) {
				player2WallIndex = 1;
			}
			if (player2FloorIndex > 3) {
				player2WallIndex = 1;
			}
			Destroy (tempReference2);
			if (wallMenu1.activeSelf == true || wallMenu2.activeSelf == true) {
				UpdateWallMenu (2);
			}
			if (floorMenu1.activeSelf == true || floorMenu2.activeSelf == true) {
				UpdateFloorMenu (2);
			}
		}
	}

	void CloseItemMenu(int playerID) {
		if (playerID == 1) {
			wallMenu1.SetActive (false);
			floorMenu1.SetActive (false);
		}
		if (playerID == 2) {
			wallMenu2.SetActive (false);
			floorMenu2.SetActive (false);
		}
	}
}
