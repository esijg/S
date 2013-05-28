using UnityEngine;
using System.Collections;

public class MoveWater : MonoBehaviour {
	public static bool isMoving = false;
	public float middleY = 0.0f;
	public float bottomY = 0.0f;
	public float topY = 0.0f;
	
	public float maxDistanceDelta = 1.0f;
	public bool testBottom = false;
	public bool testTop = false;
	
	public GameObject waterObject;
	
	bool movedToBottom = false;
	public static bool movedToTop = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void FixedUpdate()
	{
		if (testTop)
		{
						movedToTop = true;
			waterObject.collider.enabled = true;
			WorldState.teleported = true;
			isMoving = true;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, topY, transform.position.z), maxDistanceDelta*10);		

			return;
		}
		if ( (!WorldState.throwSolved || !WorldState.specificSolved || !WorldState.pressureSolved) && !movedToBottom && !movedToTop)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, middleY, transform.position.z), maxDistanceDelta);		
		}
		else if  ((WorldState.throwSolved && WorldState.specificSolved && WorldState.pressureSolved && !WorldState.stackSolved && !movedToTop) || (movedToBottom && !WorldState.stackSolved && !movedToTop))
		{
			movedToBottom = true;
			isMoving = true;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, bottomY, transform.position.z), maxDistanceDelta);		
			if (transform.position.y <= bottomY+1) waterObject.collider.enabled = false;
		}
		else if ((WorldState.throwSolved && WorldState.specificSolved && WorldState.pressureSolved && WorldState.stackSolved&& WorldState.teleported) || movedToTop)
		{
			movedToTop = true;
			waterObject.collider.enabled = true;
			isMoving = true;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, topY, transform.position.z), maxDistanceDelta*10);		
		}
	}
}
