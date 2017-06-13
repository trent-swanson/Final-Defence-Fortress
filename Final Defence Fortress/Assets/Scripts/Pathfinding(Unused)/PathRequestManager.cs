using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class PathRequestManager : MonoBehaviour {

	Queue<PathResult> results = new Queue<PathResult>();

	static PathRequestManager instance;
	Pathfinding pathfinding;

	void Awake() {
		instance = this;
		pathfinding = GetComponent<Pathfinding> ();
	}

	void Update() {
		if(results.Count > 0) {
			int itemsInQueue = results.Count;
			lock (results) {
				for (int i = 0; i < itemsInQueue; i++) {
					PathResult result = results.Dequeue ();
					result.callBack (result.path, result.success);
				}
			}
		}
	}

	public static void RequestPath(PathRequest request) {
		ThreadStart threadStart = delegate {
			instance.pathfinding.FindPath (request, instance.FinishedProcessingPath);
		};
		threadStart.Invoke ();
	}

	public void FinishedProcessingPath(PathResult result) {
		lock (results) {
			results.Enqueue (result);
		}
	}
}

public struct PathResult {
	public Vector3[] path;
	public bool success;
	public Action<Vector3[], bool> callBack;

	public PathResult(Vector3[] p_path, bool p_success, Action<Vector3[], bool> p_callBack) {
		path = p_path;
		success = p_success;
		callBack = p_callBack;
	}
}

public struct PathRequest {
	public Vector3 pathStart;
	public Vector3 pathEnd;
	public Action<Vector3[], bool> callBack;

	public PathRequest(Vector3 p_start, Vector3 p_end, Action<Vector3[], bool> p_callBack) {
		pathStart = p_start;
		pathEnd = p_end;
		callBack = p_callBack;
	}
}
