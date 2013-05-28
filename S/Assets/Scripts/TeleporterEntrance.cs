using UnityEngine;
using System.Collections;

public class TeleporterEntrance : MonoBehaviour {

	public Transform exitPoint;
	
	public bool animatingIn = false;
	
	public LightningBolt[] topStreams;
	public AudioSource onSound;
	float time = 0.0f;
	
	void Start()
	{
		renderer.enabled = false;
		collider.enabled = false;
	}
	
	void Update()
	{
		if (WorldState.streamsSolved == 4)
		{
			if (!animatingIn)
			{
				foreach (LightningBolt bolt in topStreams)
				{
					bolt.enabled = true;
				}
				onSound.Play();
				time = Time.time;
				animatingIn = true;
			}
			
			renderer.enabled = true;
			collider.enabled = true;
		}
		
		if (animatingIn)
		{
			if (time == 0.0f) time = Time.time;
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one*4, (Time.time-time)/1.0f);
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			MoveWater.movedToTop = true;
			WorldState.teleported = true;
			collider.gameObject.transform.position = exitPoint.transform.position;
		}
	}
}
