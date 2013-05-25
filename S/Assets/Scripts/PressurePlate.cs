using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	
	public float neededWeight = 10.0f;
	public float currentWeight = 0.0f;
	public ParticleEmitter activatedSystem;
	Vector3 onPosition, offPosition;
	bool activated = false;
	
	float activationTime = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		offPosition = transform.position;
		onPosition = transform.position - transform.up*0.3f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (activated)
		{
			transform.position = Vector3.Lerp(offPosition, onPosition, (Time.time - activationTime)/1.0f);
		}
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (activated)return;
		if (collision.gameObject.tag == "SoundCube")
		{
			currentWeight+=collision.gameObject.transform.localScale.magnitude;
			
			if (currentWeight >= neededWeight) 
			{
				if (!activated)
				{
					WorldState.streamsSolved++;
					activatedSystem.enabled = true;
					activationTime = Time.time;
					activated = true;
				}
			}
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "SoundCube")
		{
			currentWeight-=collision.gameObject.transform.localScale.magnitude;
		
		}
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "SoundCube")
		{
			currentWeight+=collider.gameObject.transform.localScale.magnitude;
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "SoundCube")
		{
			currentWeight-=collider.gameObject.transform.localScale.magnitude;
		}
		
	}
}
