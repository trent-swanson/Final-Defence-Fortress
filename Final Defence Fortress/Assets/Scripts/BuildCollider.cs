using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCollider : MonoBehaviour {

	Vector3 sizeOfObject;

	void Start() {
		sizeOfObject = transform.parent.parent.parent.GetComponent<Collider> ().bounds.size;
	}

	void OnTriggerEnter(Collider other) {
		//snapping
		if (BuildingManager.isBuilding && other.tag == "Building" && !other.GetComponent<BuildObject>().isSnapped) {
			float sizeX = sizeOfObject.x;
			float sizeY = sizeOfObject.y;
			float sizeZ = sizeOfObject.z;

			switch(transform.tag) {
			case "EastCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x + sizeX, transform.parent.parent.parent.position.y, transform.parent.parent.parent.position.z);
				break;
			case "WestCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x - sizeX, transform.parent.parent.parent.position.y, transform.parent.parent.parent.position.z);
				break;
			case "NorthCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x, transform.parent.parent.parent.position.y, transform.parent.parent.parent.position.z + sizeZ);
				break;
			case "SouthCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x, transform.parent.parent.parent.position.y, transform.parent.parent.parent.position.z - sizeZ);
				break;
			case "UpCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x, transform.parent.parent.parent.position.y + sizeY, transform.parent.parent.parent.position.z);
				break;
			case "DownCollider":
				other.transform.position = new Vector3 (transform.parent.parent.parent.position.x, transform.parent.parent.parent.position.y - sizeY, transform.parent.parent.parent.position.z);
				break;
			}
			other.GetComponent<BuildObject> ().isSnapped = true;
		}
	}

	void OnTriggerStay(Collider other) {
		//disable collider if placed object is adjacent
		if(other.tag == "Building" && other.GetComponent<BuildObject>().isPlaced) {
			gameObject.SetActive (false);
		}
	}
}
