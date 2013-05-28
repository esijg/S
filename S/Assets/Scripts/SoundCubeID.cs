using UnityEngine;
using System.Collections;

public class SoundCubeID : MonoBehaviour {

	public int id = 0;
	public bool thrown = false;
	
	bool colliding = false;
	bool floating = true;
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "Water" && thrown)
		{
			colliding = true;
			transform.parent = GameObject.Find("WaterLine").transform;
		}
		else if (collider.gameObject.name == "Water") colliding = true;
		
	}
	
	void OnTriggerExit(Collider collider)
	{
		if ( collider.gameObject.name == "Water")
		{
	
		}
	}
	
	void Update()
	{
		if (MoveWater.movedToTop && transform.position.y < 0)
		{
			if (transform.parent != null )transform.parent = null; 
		}
		
		if (colliding)
		{
			if (transform.parent == null) transform.parent = GameObject.Find("WaterLine").transform;
		}
	}
	
	public void Throw()
	{
		thrown = true;
		if (colliding)
		{
			transform.parent = GameObject.Find("WaterLine").transform;
		}

	}
	
	
	
	
	
	
}
