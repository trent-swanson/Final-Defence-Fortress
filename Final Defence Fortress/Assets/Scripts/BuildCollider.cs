using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCollider : MonoBehaviour {

	//vector that stores size of a object
	Vector3 sizeOfObject;
	//reference to building parent transform
	Transform buildingParentTransform;

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
		buildingParentTransform = transform.parent.parent.parent.transform;
		sizeOfObject = buildingParentTransform.GetComponent<Collider> ().bounds.size;
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerEnter()
	// If building set object enters trigger snap that object to the relavent position based
	// on where the trigger is
	//
	// Param:
	//		Collider other - reference to the object that entered trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) {
		//snapping
		//if floor object
		if (other.tag == "Building" && !other.GetComponent<BuildObject>().isSnapped) {
			float sizeX = sizeOfObject.x;
			float sizeY = sizeOfObject.y;
			float sizeZ = sizeOfObject.z;

			BuildObject otherBuildObject = other.GetComponent<BuildObject> ();

			switch(transform.tag) {
			case "EastCollider":
				if (otherBuildObject.objectType == BuildObject.enumObjectType.floor) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x + sizeX, buildingParentTransform.position.y, buildingParentTransform.position.z);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.stair) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x + sizeX, buildingParentTransform.position.y + 3, buildingParentTransform.position.z);
					other.transform.eulerAngles = new Vector3 (0, -90, 0);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.wall) {
					if (buildingParentTransform.GetChild(0).GetChild(1).GetChild(0).GetComponent<WallPoint>().hasWall == false) {
						other.transform.position = buildingParentTransform.GetChild(0).GetChild(1).GetChild(0).transform.position;
						other.transform.rotation = buildingParentTransform.GetChild(0).GetChild(1).GetChild(0).transform.rotation;
						other.GetComponent<BuildObject> ().isSnapped = true;
					}
				}
				break;
			case "WestCollider":
				if (otherBuildObject.objectType == BuildObject.enumObjectType.floor) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x - sizeX, buildingParentTransform.position.y, buildingParentTransform.position.z);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.stair) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x + sizeX, buildingParentTransform.position.y + 3, buildingParentTransform.position.z);
					other.transform.eulerAngles = new Vector3 (0, 90, 0);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.wall) {
					if (buildingParentTransform.GetChild(0).GetChild(1).GetChild(1).GetComponent<WallPoint>().hasWall == false) {
						other.transform.position = buildingParentTransform.GetChild(0).GetChild(1).GetChild(1).transform.position;
						other.transform.rotation = buildingParentTransform.GetChild(0).GetChild(1).GetChild(1).transform.rotation;
						other.GetComponent<BuildObject> ().isSnapped = true;
					}
				}
				break;
			case "NorthCollider":
				if (otherBuildObject.objectType == BuildObject.enumObjectType.floor) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x, buildingParentTransform.position.y, buildingParentTransform.position.z + sizeZ);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.stair) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x + sizeX, buildingParentTransform.position.y + 3, buildingParentTransform.position.z);
					other.transform.eulerAngles = new Vector3 (0, 180, 0);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.wall) {
					if (buildingParentTransform.GetChild(0).GetChild(1).GetChild(2).GetComponent<WallPoint>().hasWall == false) {
						other.transform.position = buildingParentTransform.GetChild(0).GetChild(1).GetChild(2).transform.position;
						other.transform.rotation = buildingParentTransform.GetChild(0).GetChild(1).GetChild(2).transform.rotation;
						other.GetComponent<BuildObject> ().isSnapped = true;
					}
				}
				break;
			case "SouthCollider":
				if (otherBuildObject.objectType == BuildObject.enumObjectType.floor) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x, buildingParentTransform.position.y, buildingParentTransform.position.z - sizeZ);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.stair) {
					other.transform.position = new Vector3 (buildingParentTransform.position.x + sizeX, buildingParentTransform.position.y + 3, buildingParentTransform.position.z);
					other.transform.eulerAngles = new Vector3 (0, 0, 0);
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				if (otherBuildObject.objectType == BuildObject.enumObjectType.wall) {
					if (buildingParentTransform.GetChild(0).GetChild(1).GetChild(3).GetComponent<WallPoint>().hasWall == false) {
						other.transform.position = buildingParentTransform.GetChild(0).GetChild(1).GetChild(3).transform.position;
						other.transform.rotation = buildingParentTransform.GetChild(0).GetChild(1).GetChild(3).transform.rotation;
						other.GetComponent<BuildObject> ().isSnapped = true;
					}
				}
				break;
			case "WallColliderUp":
				if (otherBuildObject.objectType == BuildObject.enumObjectType.floor) {
					other.transform.position = buildingParentTransform.GetChild(0).GetChild(1).transform.position;
					other.transform.rotation = buildingParentTransform.GetChild(0).GetChild(1).transform.rotation;
					other.GetComponent<BuildObject> ().isSnapped = true;
				}
				break;
			}
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerStay()
	// If building set object and isPlaced and is of objectType floor turn this trigger collider off
	//
	// Param:
	//		Coliider other - reference to the object that is in trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerStay(Collider other) {
		//disable collider if placed object is adjacent
		if((other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.floor) || (other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.stair)) {
			gameObject.SetActive (false);
		}
	}
}
