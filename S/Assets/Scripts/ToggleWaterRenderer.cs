using UnityEngine;
using System.Collections;

public class ToggleWaterRenderer : MonoBehaviour {

	
	public GameObject topWater;
	public GameObject bottomWater;
	
	bool topEnabled = true;
	public ScreenOverlay waterOverlay;
	public Transform playerPosition;
	public AudioReverbZone waterReverbZone;
	
	void Update()
	{
		collider.enabled = true;
		if ( playerPosition.position.y < bottomWater.transform.position.y)
		{
			if (topEnabled)
			{
				topEnabled = false;
				waterReverbZone.enabled = true;
				topWater.renderer.enabled = false;
				bottomWater.renderer.enabled = true;
				waterOverlay.enabled = true;
			}
		}
		
		if (playerPosition.position.y > bottomWater.transform.position.y)
		{
			if (!topEnabled)
			{
				topEnabled = true;
				waterReverbZone.enabled = false;
				topWater.renderer.enabled = true;
				bottomWater.renderer.enabled = false;
				waterOverlay.enabled = false;

			}
		}
		
	}
}
