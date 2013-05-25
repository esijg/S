using UnityEngine;
using System.Collections;

public class MoveWater : MonoBehaviour {
	
	public float middleY = 0.0f;
	public float bottomY = 0.0f;
	public float topY = 0.0f;
	
	public float maxDistanceDelta = 1.0f;
	public bool testBottom = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void FixedUpdate()
	{
			if (WorldState.streamsSolved < 3 && !testBottom)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, middleY, transform.position.z), maxDistanceDelta);		
		}
		else if (WorldState.streamsSolved == 3 || testBottom)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, bottomY, transform.position.z), maxDistanceDelta);		
			if (transform.position.y <= bottomY+2) collider.enabled = false;
		}
		else if (WorldState.streamsSolved == 4)
		{
			collider.enabled = true;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, topY, transform.position.z), maxDistanceDelta);		
		}
	}
}
