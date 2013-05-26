using UnityEngine;
using System.Collections;

public class NightPedestal : MonoBehaviour {
	
	float time;
	public Light sun;
	bool activated = false;
	float animDuration = 8.0f;
	public GameObject moon;
	GameObject cube;
	Color lightColor;
	Color camColor;
	Color fogColor;
	public LightningBolt nightBolt;
	public ParticleRenderer[] sideBolts;
	int iter = 0;
	bool occupied = false;
	
	public Material nightMaterial;
	// Use this for initialization
	void Start () {
		time = Time.time;
		lightColor = RenderSettings.ambientLight;
		camColor = Camera.mainCamera.backgroundColor;
		fogColor = RenderSettings.fogColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (occupied && !activated)
		{
			if (cube.GetComponent<SoundCubeID>().thrown)
			{
				Debug.Log("cube dropped");
				time = Time.time;
				activated = true;
				moon.SetActive(true);
				cube.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				cube.collider.enabled = false;
				cube.rigidbody.velocity = Vector3.zero;
				cube.transform.parent = this.transform;
				cube.transform.localPosition = Vector3.zero;
				cube.rigidbody.useGravity = false;
				cube.GetComponent<Buoyancy>().enabled = false;
				Invoke("KillFog", 1.0f);
				nightBolt.enabled = true;
			}
			
		}
		
		if (activated)
		{
			Camera.mainCamera.backgroundColor = Color.Lerp(camColor, Color.black, (Time.time - time)/animDuration);
			sun.intensity-= 0.001f;
			RenderSettings.ambientLight = Color.Lerp(lightColor, Color.black, (Time.time - time)/animDuration);
			RenderSettings.fogColor = Color.Lerp(fogColor, Color.clear, (Time.time - time)/animDuration);
			cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition, new Vector3(0.009505763f, 0.1321318f, -0.05523976f), Time.deltaTime);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "SoundCube")
		{
			occupied = true;
			cube = other.gameObject;

			if (other.gameObject.GetComponent<SoundCubeID>().thrown){
				Debug.Log("cube dropped");
				time = Time.time;
				activated = true;
				moon.SetActive(true);
				cube.GetComponent<Buoyancy>().enabled = false;
				cube.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				cube.collider.enabled = false;
				cube.rigidbody.velocity = Vector3.zero;
				cube.transform.parent = this.transform;
				cube.transform.localPosition = Vector3.zero;
				cube.rigidbody.useGravity = false;
				Invoke("KillFog", 1.0f);
				nightBolt.enabled = true;
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "SoundCube")
		{
			occupied = false;
		}
	}
	
	void KillFog()
	{
		if (iter < sideBolts.Length)
		{
			sideBolts[iter++].material = nightMaterial;
			Invoke("KillFog", 1.0f);
		}
		else RenderSettings.fog = false;
	}
}
