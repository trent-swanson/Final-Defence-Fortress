using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPoint : MonoBehaviour {

	//check if this wallPoint has a wall
	[HideInInspector]
	public bool hasWall = false;

	//--------------------------------------------------------------------------------------
	//	OnTriggerStay()
	// If building set object and isPlaced then this point has a wall
	//
	// Param:
	//		Collider other - reference to object that is in trigger
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void OnTriggerStay(Collider other) {
		//disable collider if placed object is adjacent
		if(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced) {
			hasWall = true;
		}
	}

}
