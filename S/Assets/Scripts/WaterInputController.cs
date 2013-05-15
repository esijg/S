using UnityEngine;
using System.Collections;

public class WaterInputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Water")
		{
			this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = false;
			rigidbody.useGravity = true;
			rigidbody.velocity = Vector3.up*-4;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	}
}
