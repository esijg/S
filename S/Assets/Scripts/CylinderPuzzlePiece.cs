using UnityEngine;
using System.Collections;

public class CylinderPuzzlePiece : MonoBehaviour {

	public Color onColor;
	bool activated = false;
	float time = 0.0f;
	public CylinderPuzzle parentPuzzle;
	
	void Update()
	{
		this.renderer.material.color = Color.Lerp(renderer.material.color, onColor,( Time.time-time)/1.5f);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "SoundCube")
		{
			if (!activated)
			{
				activated = true;
				time = Time.time;
				parentPuzzle.PieceActivated();
			}
		}
	}
}
