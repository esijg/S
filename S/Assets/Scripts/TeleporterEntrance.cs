using UnityEngine;
using System.Collections;

public class TeleporterEntrance : MonoBehaviour {

	public Transform exitPoint;
	
	void Start()
	{
		renderer.enabled = false;
		collider.enabled = false;
	}
	
	void Update()
	{
		if (WorldState.streamsSolved == 4)
		{
			renderer.enabled = true;
			collider.enabled = true;
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			collider.gameObject.transform.position = exitPoint.transform.position;
		}
	}
}
