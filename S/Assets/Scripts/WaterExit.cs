using UnityEngine;
using System.Collections;

public class WaterExit : MonoBehaviour {
	public WaterInputController controller;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "World" )
		{
			controller.StopFloating();
			transform.parent = null;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Water")
		{
			controller.HitWater();
			transform.parent = GameObject.Find("WaterLine").transform;
		}

	}

}
