using UnityEngine;
using System.Collections;

public class SmoothFollowTransform : MonoBehaviour {
	public Transform target;
	
	public Vector3 desiredOffsetFromTarget = Vector3.zero;
	public float maxDistanceDelta = 1.0f;
	public bool enabled = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!enabled)return;
		
		transform.position = Vector3.MoveTowards(transform.position, target.position+desiredOffsetFromTarget, maxDistanceDelta);
	}
	
	public void StartFollowing()
	{
		enabled = true;
	}
}
