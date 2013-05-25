using UnityEngine;
using System.Collections;

public class WaterExit : MonoBehaviour {
	public WaterInputController controller;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "World")
		{
			controller.StopFloating();
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Water")
		{
			controller.HitWater();
		}

	}

}
