using UnityEngine;
using System.Collections;

public class WaterInputController : MonoBehaviour {
	
	
	public Buoyancy floatScript;
	public GameObject bottomWater;
	public GameObject waterCollider;
	public bool underwater = false;
	public ConfigurableFPSWalker walkerScript;
	bool jumping = false;
	
	
	bool swimming = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space) && !jumping && transform.position.y > bottomWater.transform.position.y-1)
		{
			Debug.Log("trying to jump");
			waterCollider.rigidbody.useGravity = true;
			waterCollider.rigidbody.isKinematic = false;
			waterCollider.rigidbody.velocity = Vector3.up*25 + transform.forward*10;
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
		else if (underwater && transform.position.y > bottomWater.transform.position.y)
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
		if (underwater && !jumping)
		{
			if (Input.anyKey)
			{
				if (Input.GetKey(KeyCode.W)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.forward*2.5f;
				if (Input.GetKey(KeyCode.A)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.right*-2.5f;
				if (Input.GetKey(KeyCode.D)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.right*2.5f;
				if (Input.GetKey(KeyCode.S)) waterCollider.rigidbody.velocity = Camera.mainCamera.transform.forward*-2.5f;
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
			if (!waterCollider.rigidbody.isKinematic)waterCollider.rigidbody.velocity += Physics.gravity*Time.deltaTime*2;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Water")
		{
			
			if (WorldState.teleported)
			{
				waterCollider.transform.position = GameObject.Find("TopTeleporter").transform.position;
				transform.localPosition = Vector3.zero;
			}
			else if (!waterCollider.collider.enabled)
			{

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
		swimming = true;

	}
	
	public void StopFloating()
	{
		jumping = false;
		if (MoveWater.isMoving)
		{
			swimming = false;
		}
		if ( transform.position.y > bottomWater.transform.position.y || !swimming)
		{
			underwater = false;
			swimming = false;
			this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = true;
			waterCollider.collider.enabled = false;
			waterCollider.rigidbody.isKinematic = true;
			transform.localPosition = Vector3.zero;
		}
		
		
	}
	
	void StartFloating()
	{
	
		swimming = true;
		jumping = false;
		
		this.gameObject.GetComponent<ConfigurableFPSWalker>().enabled = false;
		
		waterCollider.transform.position = transform.position;
		transform.localPosition = Vector3.zero;
		
	
		waterCollider.rigidbody.useGravity = true;		
		waterCollider.collider.enabled = true;
		waterCollider.rigidbody.isKinematic = false;
		
	}
	
	
	void OnTriggerExit(Collider other)
	{
	}
}
