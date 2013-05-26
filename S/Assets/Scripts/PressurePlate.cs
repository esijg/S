using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	
	public float neededWeight = 10.0f;
	public float currentWeight = 0.0f;
	public ParticleRenderer activatedSystem;
	Vector3 onPosition, offPosition;
	bool activated = false;
	public AudioSource onAudio;
	public Color onColor;
	Color offColor;
	Material instancedMaterial;
	public Light onLight;
	float activationTime = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		offPosition = transform.position;
		onPosition = transform.position - transform.up*0.3f;
		instancedMaterial = new Material(renderer.material);
		instancedMaterial.name = renderer.material.name+"(instanced)";
		offColor = instancedMaterial.color;
		renderer.material = instancedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
		float weight = (currentWeight > neededWeight)?neededWeight:currentWeight;
		transform.position = Vector3.MoveTowards(transform.position, offPosition-transform.up* ( weight/neededWeight * 0.3f),0.01f);
		
		if (currentWeight >=neededWeight)
		{
			renderer.material.color = Color.Lerp(offColor, onColor, (Time.time - activationTime)/1.0f);
		}
		else 
		{
			renderer.material.color = Color.Lerp(onColor, offColor, (Time.time - activationTime)/1.0f);
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
					onLight.enabled = true;
					onAudio.enabled = true;
					onAudio.Play();
				}
			}
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "SoundCube")
		{
			currentWeight-=collision.gameObject.transform.localScale.magnitude;
			if (currentWeight < neededWeight)
			{
				activationTime = Time.time;
				activatedSystem.enabled = false;
				activated = false;
				WorldState.streamsSolved--;
				onLight.enabled = false;
				onAudio.enabled = false;
			}
		
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
