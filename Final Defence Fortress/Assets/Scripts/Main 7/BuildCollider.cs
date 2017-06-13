using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCollider : MonoBehaviour {

	//enum collider type
	public enum ColliderTypes {EastCollider, WestCollider, NorthCollider, SouthCollider, WallColliderUp, StairColliderUp, StairColliderDown, HealthCollider}
	public ColliderTypes colliderType;

	//vector that stores size of a object
	[HideInInspector]
	public Vector3 sizeOfObject;
	//reference to building parent transform
	[HideInInspector]
	public Transform buildingParentTransform;

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
	//	Start()
	// Runs when a collider enters the trigger collider
	//
	// Param:
	//		other - reference to the collider that entered the trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Building" && !other.GetComponent<BuildObject>().isPlaced) {
			other.GetComponent<BuildObject> ().SnapObject (gameObject);
		}
	}

	//--------------------------------------------------------------------------------------
	//	OnTriggerStay()
	// If building set object and isPlaced and is of a objectType specified turn this trigger collider off
	//
	// Param:
	//		Coliider other - reference to the object that is in trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerStay(Collider other) {
		//disable collider if placed object is adjacent
		if((other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.floor) ||
			(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.stair) ||
			(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.spikeFloor) ||
			(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.spikeWall) ||
			(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced && other.GetComponent<BuildObject>().objectType == BuildObject.enumObjectType.slowFloor)) {
			gameObject.SetActive (false);
		}
	}
}
