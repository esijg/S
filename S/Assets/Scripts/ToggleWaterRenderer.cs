using UnityEngine;
using System.Collections;

public class ToggleWaterRenderer : MonoBehaviour {

	
	public GameObject topWater;
	public GameObject bottomWater;
	
	bool topEnabled = true;
	public ScreenOverlay waterOverlay;
	public Transform playerPosition;
	
	void Update()
	{
	
		if ( playerPosition.position.y < bottomWater.transform.position.y)
		{
			if (topEnabled)
			{
				topEnabled = false;
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
				topWater.renderer.enabled = true;
				bottomWater.renderer.enabled = false;
				waterOverlay.enabled = false;

			}
		}
		
	}
}
