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
		if ( Vector3.Distance(transform.position, target.position) > desiredOffsetFromTarget.magnitude*5)
		{
			Vector3 desired = target.position + target.forward*-15+desiredOffsetFromTarget;
			desired = new Vector3(desired.x, target.position.y+desiredOffsetFromTarget.y, desired.z);
			transform.position = Vector3.MoveTowards(transform.position, desired, maxDistanceDelta*10);
		}
		if ( Vector3.Distance(transform.position, target.position) > desiredOffsetFromTarget.magnitude)
		{
			Vector3 desired = target.position + target.forward*-15+desiredOffsetFromTarget;
			desired = new Vector3(desired.x, target.position.y+desiredOffsetFromTarget.y, desired.z);
			transform.position = Vector3.MoveTowards(transform.position, desired, maxDistanceDelta);
		}
	}
	
	public void StartFollowing()
	{
		enabled = true;
	}
}
