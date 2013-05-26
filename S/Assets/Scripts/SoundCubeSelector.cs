using UnityEngine;
using System.Collections;

public class SoundCubeSelector : MonoBehaviour {
	
	public SoundCubeCreator soundCubeCreator;
	public GUITexture vignetteTexture;
	public Texture2D[] vignetteTextures;
	public GameObject[] soundCubePrefabs;
	
	int selectedIndex = 0;
	float time = 0.0f;
	float fadeOutCountdown = 0.5f;
	bool fadingOut = false;
	bool fadingIn = false;
	void Start()
	{
		SelectWeapon(selectedIndex);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (selectedIndex - 1 >= 0)
			{
				selectedIndex--;
			}
	
			else 
			{
				//selectedIndex = soundCubePrefabs.Length-1;
			}
			vignetteTexture.texture = vignetteTextures[selectedIndex];
			SelectWeapon(selectedIndex);
			fadingOut = true;
			fadeOutCountdown = 0.5f;
			
		}
	
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (selectedIndex + 1 < soundCubePrefabs.Length)
			{
				selectedIndex++;
			}
	
			else 
			{
				//selectedIndex = 0;
			}
			
			vignetteTexture.texture = vignetteTextures[selectedIndex];
	        SelectWeapon(selectedIndex);
			fadingOut = true;
			fadeOutCountdown = 0.5f;

		}
	/*	fadeOutCountdown -= Time.deltaTime;
		if( fadingOut)	vignetteTexture.color = new Color(1.0f, 1.0f, 1.0f, 2.0f * fadeOutCountdown);
		
		if ( fadeOutCountdown <= 0.1f ){
			fadingIn = true;
			fadingOut = false;
		}
		
		if (fadingIn)
		{
			fadeOutCountdown+=Time.deltaTime;
			vignetteTexture.color = new Color(1.0f, 1.0f, 1.0f, 2.0f * fadeOutCountdown);
			if ( fadeOutCountdown >= 0.5f )
			{
				fadingIn = false;
				fadingOut = false;
				fadeOutCountdown = 0.5f;
			}
		}
		*/
	}
	
	void SelectWeapon(int index)
	{
		soundCubeCreator.SetSelectedPrefab(soundCubePrefabs[index]);
	}
}