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
		if (activated)
		{
			Camera.mainCamera.backgroundColor = Color.Lerp(camColor, Color.black, (Time.time - time)/animDuration);
			sun.intensity-= 0.001f;
			RenderSettings.ambientLight = Color.Lerp(lightColor, Color.black, (Time.time - time)/animDuration);
			RenderSettings.fogColor = Color.Lerp(fogColor, Color.clear, (Time.time - time)/animDuration);
			cube.transform.localPosition = Vector3.Lerp(cube.transform.position, new Vector3(0.009505763f, 0.1321318f, -0.05523976f), Time.deltaTime);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "SoundCube")
		{
			time = Time.time;
			activated = true;
			moon.SetActive(true);
			other.gameObject.rigidbody.velocity = Vector3.zero;
			other.gameObject.transform.parent = this.transform;
			cube = other.gameObject;
			Invoke("KillFog", animDuration);
		}
	}
	
	void KillFog()
	{
			RenderSettings.fog = false;
	}
}
