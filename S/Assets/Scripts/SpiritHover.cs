using UnityEngine;
using System.Collections;


public class SpiritStatus
{
	public static bool powered = false;
	public static bool followingGaze = false;
	
	public static bool followingPlayer = false;
	static int state = 0;
	
	public static void AdvanceState()
	{
		if (state == 0)
		{
			state++;
			powered = true;
		}
		else if ( state == 1)
		{
			state++;
			followingGaze = true;
		}
		else if ( state ==2 )
		{
			followingPlayer = true;
		}
	}
}

public class SpiritHover : MonoBehaviour {
	
	bool goingUp = false;
	float delta = 0.0f;
	public Transform player;
	public GameObject pedestal;
	public ParticleSystem particleSystem;
	public SmoothFollowTransform followScript;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( SpiritStatus.followingGaze)
		{	
			transform.LookAt(player.transform.position);
			transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y+90.0f, 0.0f);
		}
		
		if ( SpiritStatus.powered)
		{
			if (particleSystem.isPlaying == false)
			{
				particleSystem.Play();
				Destroy(pedestal);
			}
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
		
		if (SpiritStatus.followingPlayer)
		{
			if (followScript.enabled == false )followScript.StartFollowing();
		}
	}
}
