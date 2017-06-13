using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BuildObject : MonoBehaviour {

	//a LayerMask object
	public LayerMask layerMask;

	//enum of building set object types
	public enum enumObjectType {floor, spikeFloor, slowFloor, wall, spikeWall, stair, healthpack};
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
	//reference for raycast starting point
	Vector3 rayOrigin;
	//reference for raycast end point
	Vector3 rayDirection;
	//reference to player1 controller
	PlayerController player1;
	//reference to player2 controller
	PlayerController player2;

	//index of player that instantiate this object
	public int playerNumber = 0;

	//Color of object when you can and cant place it
	public Color canPlace;
	public Color cantPlace;

	//Close item menu event
	public delegate void OnCloseItemMenu (int p_ID);
	public static event OnCloseItemMenu onCloseItemMenu;

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
		PlayerController.onExit += ExitPlaceObject;
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
		PlayerController.onExit -= ExitPlaceObject;
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
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
	}

	//--------------------------------------------------------------------------------------
	//	PlaceObject()
	// If snapped to another object place object, place object
	//
	// Param:
	//		int p_ID - which player
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void PlaceObject(int p_ID) {
		//place object
		if (playerNumber == p_ID) {
			if (isSnapped) {
				transform.GetChild (1).gameObject.SetActive (false);
				transform.GetChild (0).gameObject.SetActive (true);
				isPlaced = true;
				if (playerNumber == 1) {
					PlayerController player = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
					player.playerState = PlayerController.state.NotBuilding;
					player.isBuilding = false;
					if (objectType == enumObjectType.floor || objectType == enumObjectType.wall) {
						player.gold -= 20;
					} else if (objectType == enumObjectType.healthpack) {
						player.gold -= 65;
					} else if (objectType == enumObjectType.stair) {
						player.gold -= 35;
					} else {
						player.gold -= 50;
					}
					player.UpdateGold ();
					if (onCloseItemMenu != null) {
						onCloseItemMenu (playerNumber);
					}
				} else if (playerNumber == 2) {
					PlayerController player = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
					player.playerState = PlayerController.state.NotBuilding;
					player.isBuilding = false;
					if (objectType == enumObjectType.floor || objectType == enumObjectType.wall) {
						player.gold -= 20;
					} else if (objectType == enumObjectType.healthpack) {
						player.gold -= 65;
					} else if (objectType == enumObjectType.stair) {
						player.gold -= 35;
					} else {
						player.gold -= 50;
					}
					player.UpdateGold ();
					if (onCloseItemMenu != null) {
						onCloseItemMenu (playerNumber);
					}
				}
			}
			else {
				if (playerNumber == 1) {
					PlayerController player = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
					player.playerState = PlayerController.state.Placing;
					player.isBuilding = true;
				} else if (playerNumber == 2) {
					PlayerController player = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
					player.playerState = PlayerController.state.Placing;
					player.isBuilding = true;
				}
			}
			gameObject.GetComponent<BuildObject> ().enabled = false;
		}
	}

	//--------------------------------------------------------------------------------------
	//	ExitPlaceObject()
	// Exit placing object and return to not building state
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void ExitPlaceObject(int p_ID) {
		if(!isPlaced) {
			if (playerNumber == 1 && p_ID == 1) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.NotBuilding;
				player.isBuilding = false;
				Destroy (this.gameObject);
			} else if (playerNumber == 2 && p_ID == 2) {
				PlayerController player = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
				player.playerState = PlayerController.state.NotBuilding;
				player.isBuilding = false;
				Destroy (this.gameObject);
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
				rayOrigin = player1.crosshair.position;
				rayDirection = player1.aimDirection;
			} else if (playerNumber == 2) {
				rayOrigin = player2.crosshair.position;
				rayDirection = player2.aimDirection;
			}

			//while not placed follow mouse position
			if(!isPlaced && !isSnapped) {
				float rayLength = 50;
				Ray ray = new Ray(rayOrigin, rayDirection);
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
				float rayLength = 50;
				Ray ray = new Ray(rayOrigin, rayDirection);
				Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, rayLength)) {
					//if look position not == to object position un-snap
					if (Vector3.Distance(hit.point,transform.position) > 6.5f) {
						isSnapped = false;
					}
				}
			}

			//can place colour change
			if (isSnapped) {
				if (objectType == enumObjectType.stair) {
					transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material.color = canPlace;
					transform.GetChild (1).GetChild(1).GetComponent<Renderer> ().material.color = canPlace;
					transform.GetChild (1).GetChild(2).GetComponent<Renderer> ().material.color = canPlace;
				} else if (objectType == enumObjectType.spikeFloor || objectType == enumObjectType.spikeWall) {
					transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material.color = canPlace;
					transform.GetChild (1).GetChild(1).GetComponent<Renderer> ().material.color = canPlace;
				} else {
					transform.GetChild (1).GetComponent<Renderer> ().material.color = canPlace;
				}
			} else {
				if (objectType == enumObjectType.stair) {
					transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material.color = cantPlace;
					transform.GetChild (1).GetChild(1).GetComponent<Renderer> ().material.color = cantPlace;
					transform.GetChild (1).GetChild(2).GetComponent<Renderer> ().material.color = cantPlace;
				} else if (objectType == enumObjectType.spikeFloor || objectType == enumObjectType.spikeWall) {
					transform.GetChild (1).GetChild(0).GetComponent<Renderer> ().material.color = cantPlace;
					transform.GetChild (1).GetChild(1).GetComponent<Renderer> ().material.color = cantPlace;
				} else {
					transform.GetChild (1).GetComponent<Renderer> ().material.color = cantPlace;
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------
	//	SnapObject()
	// Position this object based on objectType and which collider it hit
	//
	// Param:
	//		snapCollider - reference to the collider object this object entered
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void SnapObject(GameObject snapCollider) {
		BuildCollider buildCollider = snapCollider.GetComponent<BuildCollider> ();
		Transform snapToParent = buildCollider.buildingParentTransform;
		float sizeX = buildCollider.sizeOfObject.x;
		float sizeZ = buildCollider.sizeOfObject.z;

		switch(buildCollider.colliderType) {
		case BuildCollider.ColliderTypes.EastCollider:
			if (objectType == enumObjectType.floor) {
				transform.position = new Vector3 (snapToParent.position.x + sizeX, snapToParent.position.y, snapToParent.position.z);
				isSnapped = true;
			}
			if (objectType == enumObjectType.stair) {
				if (snapToParent.GetChild(0).GetChild(2).GetChild(0).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(2).GetChild(0).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(2).GetChild(0).transform.rotation;
					isSnapped = true;
				}
			}
			if (objectType == enumObjectType.wall) {
				if (snapToParent.GetChild(0).GetChild(1).GetChild(0).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(1).GetChild(0).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(1).GetChild(0).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.WestCollider:
			if (objectType == enumObjectType.floor) {
				transform.position = new Vector3 (snapToParent.position.x - sizeX, snapToParent.position.y, snapToParent.position.z);
				isSnapped = true;
			}
			if (objectType == enumObjectType.stair) {
				if (snapToParent.GetChild(0).GetChild(2).GetChild(1).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(2).GetChild(1).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(2).GetChild(1).transform.rotation;
					isSnapped = true;
				}
			}
			if (objectType == enumObjectType.wall) {
				if (snapToParent.GetChild(0).GetChild(1).GetChild(1).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(1).GetChild(1).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(1).GetChild(1).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.NorthCollider:
			if (objectType == enumObjectType.floor) {
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y, snapToParent.position.z + sizeZ);
				isSnapped = true;
			}
			if (objectType == enumObjectType.stair) {
				if (snapToParent.GetChild(0).GetChild(2).GetChild(2).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(2).GetChild(2).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(2).GetChild(2).transform.rotation;
					isSnapped = true;
				}
			}
			if (objectType == enumObjectType.wall) {
				if (snapToParent.GetChild(0).GetChild(1).GetChild(2).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(1).GetChild(2).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(1).GetChild(2).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.SouthCollider:
			if (objectType == enumObjectType.floor) {
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y, snapToParent.position.z - sizeZ);
				isSnapped = true;
			}
			if (objectType == enumObjectType.stair) {
				if (snapToParent.GetChild(0).GetChild(2).GetChild(3).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(2).GetChild(3).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(2).GetChild(3).transform.rotation;
					isSnapped = true;
				}
			}
			if (objectType == enumObjectType.wall) {
				if (snapToParent.GetChild(0).GetChild(1).GetChild(3).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(1).GetChild(3).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(1).GetChild(3).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.WallColliderUp:
			if (objectType == enumObjectType.floor) {
				transform.position = snapToParent.GetChild(0).GetChild(1).transform.position;
				transform.rotation = snapToParent.GetChild(0).GetChild(1).transform.rotation;
				isSnapped = true;
			}
			if (objectType == enumObjectType.spikeWall) {
				transform.rotation = snapToParent.GetChild(0).GetChild(2).transform.rotation;
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y - 2f, snapToParent.position.z);
				isSnapped = true;
			}
			break;
		case BuildCollider.ColliderTypes.StairColliderUp:
			if (objectType == enumObjectType.floor) {
				if (snapToParent.GetChild(0).GetChild(4).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(4).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(4).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.StairColliderDown:
			if (objectType == enumObjectType.floor) {
				if (snapToParent.GetChild(0).GetChild(5).GetComponent<WallPoint>().hasWall == false) {
					transform.position = snapToParent.GetChild(0).GetChild(5).transform.position;
					transform.rotation = snapToParent.GetChild(0).GetChild(5).transform.rotation;
					isSnapped = true;
				}
			}
			break;
		case BuildCollider.ColliderTypes.HealthCollider:
			if (objectType == enumObjectType.spikeFloor) {
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y + 0.1f, snapToParent.position.z);
				isSnapped = true;
			}
			if (objectType == enumObjectType.slowFloor) {
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y + 0.1f, snapToParent.position.z);
				isSnapped = true;
			}
			if (objectType == enumObjectType.healthpack) {
				transform.position = new Vector3 (snapToParent.position.x, snapToParent.position.y + 0.5f, snapToParent.position.z);
				isSnapped = true;
			}
			break;
		}
	}
}
