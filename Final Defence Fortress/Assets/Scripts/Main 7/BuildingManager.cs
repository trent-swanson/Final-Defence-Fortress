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
	//reference to player1 floor UI panel
	public GameObject floorMenu1;
	//reference to player1 wall UI panel
	public GameObject wallMenu1;
	//reference to player2 floor UI panel
	public GameObject floorMenu2;
	//reference to player2 wall UI panel
	public GameObject wallMenu2;
	//saving normal colour for UI panel
	public Color normalColour;
	//saving highlighted colour for UI panel
	public Color highlightColour;

	//references to building set objects
	[Space(20)]
	[Header("Building Prefabs")]
	//reference to floor prefab
	public GameObject floorPrefab;
	//reference to wall prefab
	public GameObject wallPrefab;
	//reference to stair prefab
	public GameObject stairPrefab;
	//reference to slow trap prefab
	public GameObject slowTrapPrefab;
	//reference to floor spike prefab
	public GameObject floorSpikePrefab;
	//reference to wall spike prefab
	public GameObject wallSpikePrefab;
	//reference to healthpack prefab
	public GameObject healthPack;

	//reference to player1 controller
	PlayerController player1;
	//reference to player2 controller
	PlayerController player2;

	//index of current item in player1's wall panel 
	int player1WallIndex = 1;
	//index of current item in player1's floor panel 
	int player1FloorIndex = 1;

	//index of current item in player2's wall panel 
	int player2WallIndex = 1;
	//index of current item in player2's floor panel 
	int player2FloorIndex = 1;

	//temp reference of building object instantiated
	GameObject tempReference1;
	//temp reference of building object instantiated
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
	// Choose building type to instantiate or open building menu
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

	//--------------------------------------------------------------------------------------
	//	UpdateWallMenu()
	// Update a players wall item menu to show selected item and instantiate that item
	//
	// Param:
	//		int playerNumber - index of player that called function	
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
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

	//--------------------------------------------------------------------------------------
	//	UpdateWallMenu()
	// Update a players floor item menu to show selected item and instantiate that item
	//
	// Param:
	//		int playerNumber - index of player that called function	
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
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

	//--------------------------------------------------------------------------------------
	//	Update()
	// Runs every frame, Dpad to select item in build item menu
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
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

	//--------------------------------------------------------------------------------------
	//	CloseItemMenu()
	// Close a players build item menu
	//
	// Param:
	//		int playerID - which player
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
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
