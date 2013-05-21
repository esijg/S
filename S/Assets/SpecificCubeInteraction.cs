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
	public Material cubeSkeletonMaterial;
	public AudioSource cubeSource;
	public GameObject skeleton;
	
	Color neutralColor;
	bool activating = false;
	bool failing = false;
	float failTime = 0.0f;
	// Use this for initialization
	void Start () {

		neutralColor = new Color(115/255.0f, 115/255.0f, 115/255.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (activating)
		{
			cubeSkeletonMaterial.color = Color.Lerp(neutralColor, Color.white, (Time.time - failTime)/1.0f);
		}
		
		if (failing)
		{
			
			cubeSource.volume += 0.02f;
			
			cubeSkeletonMaterial.color = Color.Lerp(neutralColor, Color.red, (Time.time - failTime)/1.0f);
			if (Time.time - failTime  > 1.0f)
			{
				failTime = Time.time;
				failing = false;
			}
		}
		else if (!activating)
		{
			if (cubeSource.volume > 0.0f) cubeSource.volume -= 0.02f;
			cubeSkeletonMaterial.color = Color.Lerp(Color.red, neutralColor, (Time.time - failTime)/1.0f);
		}
		
	}
	
	void Activate(GameObject cube)
	{
		renderer.enabled = true;
		particleSystem.Play();
		failTime = Time.time;
		cubeSource.volume = 0.0f;
		activating = true;
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
		failTime = Time.time;
		failing = true;
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
