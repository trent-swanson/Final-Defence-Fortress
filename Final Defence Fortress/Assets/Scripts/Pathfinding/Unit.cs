using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 0.5f;

	public Transform target;
	public float speed = 6f;
	public float turnSpeed = 3;
	public float turnDistance = 5f;
	public float stoppingDistance = 4;

	Path path;

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			path = new Path(waypoints, transform.position, turnDistance, stoppingDistance);
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	protected IEnumerator UpdatePath() {
		if (Time.timeSinceLevelLoad < 0.3f) {
			yield return new WaitForSeconds (0.3f);
		}
		PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;

		while(true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			if((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));
				targetPosOld = target.position;
			}
		}
	}

	protected IEnumerator FollowPath() {

		bool followingPath = true;
		int pathIndex = 0;
		transform.LookAt (new Vector3(path.lookPoints [0].x, 1, path.lookPoints [0].z));

		float speedPercent = 1;

		while (followingPath) {
			Vector2 pos2d = new Vector2 (transform.position.x, transform.position.z);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2d)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {

				if (pathIndex >= path.slowDownIndex && stoppingDistance > 0) {
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2d) / stoppingDistance);
					if (speedPercent < 0.01f) {
						followingPath = false;
					}
				}
				Vector3 lookPos = path.lookPoints [pathIndex] - transform.position;
				lookPos.y = 1;
				Quaternion targetRotation = Quaternion.LookRotation (lookPos);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			}

			yield return null;
		}
	}



}
