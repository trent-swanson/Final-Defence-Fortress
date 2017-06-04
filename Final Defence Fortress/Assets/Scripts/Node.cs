using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector3 worldPosition;
	public int gridX, gridY;
	public int movementPenalty;

	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;

	public Node(bool p_walkable, Vector3 p_worldPos, int p_gridX, int p_gridY, int p_penalty) {
		walkable = p_walkable;
		worldPosition = p_worldPos;
		gridX = p_gridX;
		gridY = p_gridY;
		movementPenalty = p_penalty;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo (nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);
		}
		return -compare;
	}

}
