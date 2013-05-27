using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	public UISprite viewControls;
	public UISprite controls;
	public UISprite hideControls;
	public UISprite logo;
	public UISprite invertedLogo;
	bool showing = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (WorldState.gameOver)
		{
			invertedLogo.enabled = true;
			invertedLogo.alpha+= 0.01f;
		}
		
		
		if (Input.GetKeyDown(KeyCode.C))
		{
			if ( !showing)
			{
				showing = true;
				controls.enabled = true;
				viewControls.enabled = false;
				hideControls.enabled = true;
			}
			else
			{
				showing = false;
				controls.enabled = false;
				viewControls.enabled = false;
				hideControls.enabled = false;
			}
		}
	}
}
