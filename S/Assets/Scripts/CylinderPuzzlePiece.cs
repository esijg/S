using UnityEngine;
using System.Collections;

public class CylinderPuzzlePiece : MonoBehaviour {
	
	static int numActivated = 0;
	
	public Color onColor;
	bool activated = false;
	float time = 0.0f;
	public CylinderPuzzle parentPuzzle;
	public Material sourceMaterial;
	public Renderer[] materialObjects;
	Material ourMaterial;
	public string name = "";
	
	public LightningBolt streamEmitter;
	
	void Start()
	{
		ourMaterial = new Material(sourceMaterial);
		ourMaterial.name = ""+name+"(instance)";
		foreach(Renderer r in materialObjects) r.material = ourMaterial;
	}
	
	void Update()
	{
		if (activated) ourMaterial.color = Color.Lerp(Color.black, onColor,( Time.time-time)/1.5f);
	}
	
	void OnTriggerEnter(Collider collision)
	{
		Debug.Log("HIT");
		if (collision.gameObject.tag == "SoundCube")
		{
			if (!activated)	
			{
				numActivated++;
				if (numActivated == 4)
				{
					GameObject.Find("Stream 2").GetComponent<ParticleRenderer>().enabled = true;
				}
				Debug.Log("Activated");
				streamEmitter.enabled = true;
				activated = true;
				time = Time.time;
				if (parentPuzzle != null)parentPuzzle.PieceActivated();
			}
		}
	}
}
