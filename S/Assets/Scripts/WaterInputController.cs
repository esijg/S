using UnityEngine;
using System.Collections;

public class WaterInputController : MonoBehaviour {
	
	
	public Buoyancy floatScript;
	public GameObject bottomWater;
	public GameObject waterCollider;
	public bool underwater = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey && floatScript.enabled)
		{
			this.gameObject.GetComponent<Buoyancy>().enabled = false;
			
		}
		else if (Input.anyKey && transform.position.y < bottomWater.transform.position.y)
		{
			underwater = true;
			waterCollider.rigidbody.isKinematic = false;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
		
		if (underwater && transform.position.y > bottomWater.transform.position.y)
		{
			underwater = false;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			waterCollider.collider.enabled = false;
			waterCollider.rigidbody.isKinematic = true;
		}
		
		if (!Input.anyKey && !floatScript.enabled)
		{
			this.gameObject.GetComponent<Buoyancy>().enabled = true;
		}
		else if (!Input.anyKey)
		{
			
		}
	}
	
	
	void FixedUpdate()
	{
		if (underwater)
		{
			if (Input.anyKey)
			{
				if (waterCollider.rigidbody.isKinematic == true) waterCollider.rigidbody.isKinematic = false;
				if (Input.GetKey(KeyCode.W)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.forward;
				if (Input.GetKey(KeyCode.A)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.right*-1;
				if (Input.GetKey(KeyCode.D)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.right;
				if (Input.GetKey(KeyCode.S)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.forward*-1;
			}
			else
			{
				if (floatScript.enabled == false)
				{
					waterCollider.rigidbody.velocity = Vector3.zero;
					floatScript.enabled = true;
					rigidbody.useGravity = true;
					rigidbody.isKinematic = false;
					waterCollider.collider.enabled = false;
					waterCollider.rigidbody.isKinematic = true;
				}
			}
		}
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
