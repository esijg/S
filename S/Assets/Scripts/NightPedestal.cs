using UnityEngine;
using System.Collections;

public class NightPedestal : MonoBehaviour {
	
	float time;
	public Light sun;
	// Use this for initialization
	void Start () {
		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Camera.mainCamera.backgroundColor = Color.Lerp(Color.white, Color.black, (Time.time - time)/15.0f);
		sun.intensity = 0.01f;
		
	}
}
