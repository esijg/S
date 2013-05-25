using UnityEngine;
using System.Collections;

public class SoundCubeID : MonoBehaviour {

	public int id = 0;
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "Water")
		{
			transform.parent = GameObject.Find("WaterLine").transform;
		}
	}
}
