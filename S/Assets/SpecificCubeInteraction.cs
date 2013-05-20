using UnityEngine;
using System.Collections;

public class SpecificCubeInteraction : MonoBehaviour {
	public int targetId = 5;
	
	bool activated = false;
	
	public AudioSource correctSoundSource;
	public ParticleRenderer renderer;
	public ParticleSystem particleSystem;
	public Transform correctCubePosition;
	public Material streamBeamMaterial;
	
	bool activating = false;
	bool failing = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (activating)
		{
		}
		
		if (failing)
		{
		}
	}
	
	void Activate(GameObject cube)
	{
		renderer.enabled = true;
		particleSystem.Play();
		
		cube.transform.position = correctCubePosition.position;
		cube.rigidbody.velocity = Vector3.zero;
		cube.rigidbody.useGravity = false;
		cube.rigidbody.isKinematic = true;
		cube.rigidbody.angularVelocity = Vector3.zero;
		cube.collider.enabled = false;
		correctSoundSource.enabled = true;
		correctSoundSource.Play();
		
	}
	
	void Fail(GameObject cube)
	{
		Destroy(cube);	
	}
	
	
	void OnTriggerEnter(Collider collider)	
	{
		if ( collider.tag == "SoundCube")
		{
			if (collider.gameObject.GetComponent<SoundCubeID>().id == targetId)
			{
				Activate(collider.gameObject);
			}
			else Fail(collider.gameObject);
		}
	}

}
