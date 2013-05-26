using UnityEngine;
using System.Collections;

public class SoundCubeID : MonoBehaviour {

	public int id = 0;
	public bool thrown = false;
	
	bool colliding = false;
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "Water" && thrown)
		{
			colliding = true;
			transform.parent = GameObject.Find("WaterLine").transform;
		}
		
	}
	
	void OnTriggerExit(Collider collider)
	{
		if ( collider.gameObject.name == "Water")
		{
			colliding = false;
		}
	}
	
	public void Throw()
	{
		thrown = true;
		if (colliding) transform.parent = GameObject.Find("WaterLine").transform;

	}
	
	
}
