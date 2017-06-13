using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	Grid grid;

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	public void FindPath(PathRequest request, Action<PathResult> callback) {
		Vector3[] wayPoints = new Vector3[0];
		bool pathSuccess = false;

		// get start and target node from world position
		Node startNode = grid.GetNodeFromWorldPoint (request.pathStart);
		Node targetNode = grid.GetNodeFromWorldPoint (request.pathEnd);

		if (startNode.walkable && targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node> (grid.MaxSize); // the set of nodes to be evaluated
			HashSet<Node> closeSet = new HashSet<Node> (); // the set of nodes already evaluated

			// add startNode to openSet
			openSet.Add (startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closeSet.Add (currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.walkable || closeSet.Contains(neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour) + neighbour.movementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour)) {
							openSet.Add (neighbour);
						} else {
							openSet.UpdateItem (neighbour);
						}
					}
				}
			}
		}
		if (pathSuccess == true) {
			wayPoints = RetracePath (startNode, targetNode);
			pathSuccess = wayPoints.Length > 0;
		}
		callback (new PathResult (wayPoints, pathSuccess, request.callBack));
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2 (path [i - 1].gridX - path [i].gridX, path [i - 1].gridY - path [i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add (path [i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray ();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int disX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int disY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (disX > disY) {
			return 14 * disY + 10 * (disX - disY);
		} else {
			return 14 * disX + 10 * (disY - disX);
		}
	}

}
