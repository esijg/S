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
	public GameObject nightBoltEnd;
	public Material nightMaterial;
	bool animateBolt = false;
	
	Color nightColor;
	// Use this for initialization
	void Start () {
		time = Time.time;
		lightColor = RenderSettings.ambientLight;
		camColor = Camera.mainCamera.backgroundColor;
		fogColor = RenderSettings.fogColor;
		nightColor = nightMaterial.color;
		nightMaterial = new Material(nightMaterial);
		nightMaterial.name = nightMaterial.name+"(instance)";
		nightBolt.gameObject.GetComponent<ParticleRenderer>().material = nightMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (animateBolt && Time.time-time < 5.0f)
		{
			nightBoltEnd.transform.Translate(Vector3.up);
		}
		
		if (occupied && !activated)
		{
			if (cube.GetComponent<SoundCubeID>().thrown)
			{
				Debug.Log("cube dropped");
				moon.SetActive(true);
				occupied = false;
				cube.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				cube.collider.enabled = false;
				cube.rigidbody.velocity = Vector3.zero;
				cube.transform.parent = this.transform;
				cube.rigidbody.useGravity = false;
				cube.GetComponent<Buoyancy>().enabled = false;

				Invoke("KillFog", 1.0f);
			}
			
		}
		
		if (moon.activeSelf)
		{
			cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition, Vector3.up*0.1f+Vector3.right*0.009505763f+Vector3.forward*-0.05523976f, Time.deltaTime*4);
			cube.transform.localRotation = Quaternion.Lerp(cube.transform.localRotation, Quaternion.identity, Time.deltaTime*4);
		}
		
		if (activated)
		{
			foreach(ParticleRenderer r in sideBolts)
			{
				if (r.maxParticleSize > 0.0f)r.maxPartileSize-=0.001f;
			}
			sun.intensity-= 0.001f;
			RenderSettings.fogDensity-= 0.0001f;
			Camera.mainCamera.backgroundColor = Color.Lerp(camColor, Color.black, (Time.time - time)/animDuration);
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
		else if (iter == sideBolts.Length)
		{
			iter++;
			nightBolt.enabled = true;
			animateBolt = true;
			Invoke("KillFog", 1.0f);

		}
		else
		{
			time = Time.time;
			activated = true;
			Invoke("UI", animDuration);
		}
	}
	
	void UI()
	{
		WorldState.gameOver = true;
	}
}
