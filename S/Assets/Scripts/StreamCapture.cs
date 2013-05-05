using UnityEngine;
using System.Collections;

public class StreamCapture : MonoBehaviour {
	public int requiredSoundCubeID = 0;
	public ParticleSystem powerCord;
	public GameObject cubeSkeletonLeft, cubeSkeletonRight;
	public Material onMaterial;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "SoundCube")
		{
			if ( other.gameObject.GetComponent<SoundCubeID>().id == requiredSoundCubeID )
			{
				other.gameObject.rigidbody.velocity = Vector3.zero;
				other.gameObject.rigidbody.isKinematic = true;
				Destroy(other.gameObject);
				
				Destroy(cubeSkeletonLeft.transform.parent.gameObject);				
				renderer.material = onMaterial;
				SpiritStatus.AdvanceState();
				powerCord.Play();
			}
			else
			{
				Destroy(other.gameObject);
			}
		}
	}
}
