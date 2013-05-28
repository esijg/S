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
	public Material lightningMaterial;
	Color neutralColor;
	bool activating = false;
	bool failing = false;
	float failTime = 0.0f;
	public GameObject[] materialObjects;
	public AudioSource completedSound2;
	Material instancedMaterial;
	public Material alpha6;
	
	// Use this for initialization
	void Start () {
		lightningMaterial.color = new Color(1.0f, 1.0f,1.0f, 0.1f);
		cubeSkeletonMaterial = new Material(cubeSkeletonMaterial);
		cubeSkeletonMaterial.name = cubeSkeletonMaterial.name+"(instanced)";
		cubeSkeletonMaterial.color = Color.black;
		neutralColor = Color.black;//new Color(115/255.0f, 115/255.0f, 115/255.0f, 1.0f);
		alpha6 = cubeSource.gameObject.renderer.material;
		foreach (GameObject r in materialObjects)
		{
			r.renderer.material = cubeSkeletonMaterial;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (activating)
		{
			cubeSkeletonMaterial.color = Color.Lerp(neutralColor, Color.white, (Time.time - failTime)/2.0f);
			lightningMaterial.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.1f), Color.white, (Time.time - failTime)/2.0f);
			cubeSource.audio.volume = 0.0f;
			cubeSource.renderer.material.color = Color.clear;
			if ( Time.time - failTime > 2.0f && particleSystem.isPlaying == false)
			{
				particleSystem.Play();

			}
			else if (Time.time - failTime > 2.0f)
			{
				cubeSource.volume -= 0.02f;

			}
			else cubeSource.volume -= 0.04f;

		}
		if (activating)return;
		if (failing)
		{
			
			cubeSource.volume += 0.04f;
			cubeSource.renderer.material.color = new Color(115/255.0f, 115/255.0f, 115/255.0f, 200.0f/255.0f);
			cubeSkeletonMaterial.color = Color.Lerp(neutralColor, Color.red, (Time.time - failTime)/1.0f);
			if (Time.time - failTime  > 1.0f)
			{
				failTime = Time.time;
				failing = false;
				cubeSource.volume -= 0.04f;
				
			}
		}
		else if (!activating)
		{
			if (cubeSource.volume > 0.0f) cubeSource.volume -= 0.02f;
			cubeSource.renderer.material.color = new Color(alpha6.color.r, alpha6.color.g, alpha6.color.b, cubeSource.renderer.material.color.a-0.01f);

			cubeSkeletonMaterial.color = Color.Lerp(Color.red, neutralColor, (Time.time - failTime)/1.0f);
		}

		
	}
	
	void Activate(GameObject cube)
	{
		renderer.enabled = true;
		failTime = Time.time;
		cubeSource.volume = 0.0f;
		activating = true;
		cube.transform.position = correctCubePosition.position;
		cube.rigidbody.velocity = Vector3.zero;
		cube.rigidbody.useGravity = false;
		cube.rigidbody.isKinematic = true;
		cube.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
		cube.rigidbody.angularVelocity = Vector3.zero;
		cube.collider.enabled = false;
		correctSoundSource.enabled = true;
		correctSoundSource.Play();
		completedSound2.Play();
		cube.audio.Stop();
		
		
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
				if (!activating)
				{
					Activate(collider.gameObject);
					WorldState.streamsSolved++;
					WorldState.specificSolved = true;
				}
			}
			else Fail(collider.gameObject);
		}
	}

}
