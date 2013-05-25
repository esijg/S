using UnityEngine;
using System.Collections;

public class WaterInputController : MonoBehaviour {
	
	
	public Buoyancy floatScript;
	public GameObject bottomWater;
	public GameObject waterCollider;
	public bool underwater = false;
	public ConfigurableFPSWalker walkerScript;
	bool jumping = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space) && !jumping && transform.position.y > bottomWater.transform.position.y-5)
		{
			Debug.Log("trying to jump");
			waterCollider.rigidbody.useGravity = true;
			waterCollider.rigidbody.isKinematic = false;
			waterCollider.rigidbody.velocity = Vector3.up*20 + transform.forward*10;
			waterCollider.rigidbody.angularDrag = 3;
			waterCollider.rigidbody.drag = 3;
			jumping = true;
			underwater = false;
		}
		
		
		
		if (Input.anyKey && floatScript.enabled)
		{
			waterCollider.GetComponent<Buoyancy>().enabled = false;
			
		}
		else if (Input.anyKey && transform.position.y < bottomWater.transform.position.y)
		{
			underwater = true;
			waterCollider.rigidbody.useGravity = false;
		}
		else if (underwater && transform.position.y > bottomWater.transform.position.y && !jumping)
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
		if (underwater && waterCollider.rigidbody.isKinematic == false && !jumping)
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
					waterCollider.rigidbody.velocity += Vector3.zero;
					floatScript.enabled = true;
					waterCollider.rigidbody.useGravity = true;
				
				}
			}
		}
		else if (jumping)
		{
			waterCollider.rigidbody.useGravity = true;
			if (!waterCollider.rigidbody.isKinematic)waterCollider.rigidbody.velocity += Physics.gravity*Time.deltaTime*Time.deltaTime;
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
		else if (jumping)
		{
			walkerScript.enabled = true;
		}
	}
	
	public void HitWater()
	{
		Debug.Log("hit water");
		underwater = false;
		jumping = false;

	}
	
	public void StopFloating()
	{
		if (jumping){
			underwater = false;
			this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = true;
			waterCollider.collider.enabled = false;
			waterCollider.rigidbody.isKinematic = true;
			transform.localPosition = Vector3.zero;
		
		}
	}
	
	void StartFloating()
	{
		
		jumping = false;
		this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = false;
		transform.parent = null;
		waterCollider.transform.position = transform.position;
		transform.parent = waterCollider.transform;
		transform.localPosition = Vector3.zero;
		waterCollider.rigidbody.useGravity = true;		
		waterCollider.collider.enabled = true;
		waterCollider.rigidbody.isKinematic = false;
	}
	
	
	void OnTriggerExit(Collider other)
	{
	}
}
