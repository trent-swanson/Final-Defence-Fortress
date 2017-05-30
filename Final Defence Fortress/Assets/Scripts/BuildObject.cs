using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildObject : MonoBehaviour {

	//a LayerMask object
	public LayerMask layerMask;

	//enum of building set object types
	public enum enumObjectType {floor, wall, roof, trap, turret};
	//enum object of objectType
	public enumObjectType objectType;

	//size of x, y, z snap amounts
	public int xSnapStep;
	public int ySnapStep;
	public int zSnapStep;

	//check if object is placed
	public bool isPlaced;
	//check if object is snapped to another object
	public bool isSnapped = false;
	//refrences to PlayerLookAt transforms
	Transform PlayerCameraLookAt;
	Transform PlayerLookAt1;
	Transform PlayerLookAt2;

	//index of player that instantiate this object
	public int playerNumber = 0;

	//Color of object when you can and cant place it
	public Color canPlace;
	public Color cantPlace;

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
		PlayerController.onPlace += PlaceObject;
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
		PlayerController.onPlace -= PlaceObject;
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
		PlayerLookAt1 = GameObject.FindGameObjectWithTag ("Player1").transform.GetChild (0).GetChild(0).GetChild(0).transform;
		PlayerLookAt2 = GameObject.FindGameObjectWithTag ("Player2").transform.GetChild (0).GetChild(0).GetChild(0).transform;
	}

	//--------------------------------------------------------------------------------------
	//	PlaceObject()
	// If snapped to another object place object
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void PlaceObject() {
		//place object
		if (isSnapped) {
			transform.GetChild (1).gameObject.SetActive (false);
			transform.GetChild (0).gameObject.SetActive (true);
			isPlaced = true;
			PlayerController.isBuilding = false;
			if (playerNumber == 1) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.NotBuilding;
			} else if (playerNumber == 2) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.NotBuilding;
			}
		}
		else {
			PlayerController.isBuilding = true;
			if (playerNumber == 1) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.Placing;
			} else if (playerNumber == 2) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.Placing;
			}
		}
	}

	//--------------------------------------------------------------------------------------
	//	Update()
	// Runs every frame, object follows raycast hit, if snapped and looking somewhere else
	// unsnap, show if object is snapped and therefore placable
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update() {
		if (playerNumber != 0) {
			if (playerNumber == 1) {
				PlayerCameraLookAt = PlayerLookAt1;
			} else if (playerNumber == 2) {
				PlayerCameraLookAt = PlayerLookAt2;
			}

			//while not placed follow mouse position
			if(!isPlaced && !isSnapped) {
				float rayLength = 50;
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
					pos.y += (transform.GetComponent<BoxCollider>().bounds.size.y / 2);

					gridSteps = Mathf.RoundToInt (pos.z / zSnapStep);
					pos.z = ((float)gridSteps) * zSnapStep;
					transform.position = pos;
				}
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
	}
}
