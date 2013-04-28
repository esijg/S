using UnityEngine;
using System.Collections;

public class SpiritHover : MonoBehaviour {
	
	bool goingUp = false;
	float delta = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (goingUp)
		{
			transform.position += Vector3.up*(0.1f * Time.deltaTime);
			delta+=0.1f * Time.deltaTime;
			if (delta > 1.0f) goingUp = false;
		}
		else
		{
			transform.position -= Vector3.up*(0.1f * Time.deltaTime);
			delta-=0.1f * Time.deltaTime;
			if (delta <= 0.0f) goingUp = true;
		}
	}
}
