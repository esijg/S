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
			waterCollider.GetComponent<Buoyancy>().enabled = false;
			
		}
		else if (Input.anyKey && transform.position.y < bottomWater.transform.position.y)
		{
			underwater = true;
			waterCollider.rigidbody.useGravity = false;
		}
		
		if (underwater && transform.position.y > bottomWater.transform.position.y)
		{
			underwater = false;
			waterCollider.rigidbody.useGravity = true;
			waterCollider.rigidbody.isKinematic = false;
		}
		
		if (!Input.anyKey && !floatScript.enabled)
		{
			waterCollider.GetComponent<Buoyancy>().enabled = true;
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
					waterCollider.rigidbody.useGravity = true;
				
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Water")
		{
			if (!waterCollider.collider.enabled)
			{
							waterCollider.collider.enabled = true;

				Invoke("StartFloating", 0.4f);
			}
		}
	}
	
	void StartFloating()
	{
		this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = false;
			transform.parent = null;
			waterCollider.transform.position = transform.position;
			transform.parent = waterCollider.transform;
			transform.localPosition = Vector3.zero;
			waterCollider.rigidbody.useGravity = true;		
			waterCollider.collider.enabled = true;
	}
	
	
	void OnTriggerExit(Collider other)
	{
	}
}
