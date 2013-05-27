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
	public GameObject cube;
	public LightningBolt streamEmitter;
	public AudioSource[] completedSounds;
	void Start()
	{
		ourMaterial = new Material(sourceMaterial);
		ourMaterial.name = ""+name+"(instance)";
		foreach(Renderer r in materialObjects) r.material = ourMaterial;
	}
	
	void Update()
	{
		if (activated)
		{
			ourMaterial.color = Color.Lerp(Color.black, onColor,( Time.time-time)/1.5f);
			cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition, Vector3.up*0.1f+Vector3.right*0.009505763f+Vector3.forward*-0.05523976f, Time.deltaTime*4);
			cube.transform.localRotation = Quaternion.Lerp(cube.transform.localRotation, Quaternion.identity, Time.deltaTime*4);
		}
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
					WorldState.streamsSolved++;
					
					foreach( AudioSource source in completedSounds)
					{
						source.Play();
					}
				}
				Debug.Log("Activated");
				streamEmitter.enabled = true;
				cube = collision.gameObject;
				
								cube.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				cube.collider.enabled = false;
				cube.rigidbody.velocity = Vector3.zero;
				cube.transform.parent = this.transform;
				cube.rigidbody.useGravity = false;
				cube.GetComponent<Buoyancy>().enabled = false;

				
				activated = true;
				time = Time.time;
				if (parentPuzzle != null)parentPuzzle.PieceActivated();
			}
		}
	}
}
