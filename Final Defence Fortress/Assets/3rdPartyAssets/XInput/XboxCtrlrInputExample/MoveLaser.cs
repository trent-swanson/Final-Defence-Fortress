using UnityEngine;
using System.Collections;

public class MoveLaser : MonoBehaviour 
{
	public float speed = 15.0f;
	private Vector3 newPosition;
	
	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject, 1.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		newPosition = transform.parent.parent.parent.position;
		newPosition = transform.parent.parent.parent.position + transform.parent.parent.parent.forward * speed * Time.deltaTime;
		transform.parent.parent.parent.position = newPosition;
	}
}
