using UnityEngine;
using System.Collections;

public class SoundCubeSelector : MonoBehaviour {
	
	public SoundCubeCreator soundCubeCreator;
	
	public GameObject[] soundCubePrefabs;
	
	int selectedIndex = 0;
	
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
				selectedIndex = soundCubePrefabs.Length-1;
			}
	
			SelectWeapon(selectedIndex);
			
		}
	
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (selectedIndex + 1 < soundCubePrefabs.Length)
			{
				selectedIndex++;
			}
	
			else 
			{
				selectedIndex = 0;
			}
	
	        SelectWeapon(selectedIndex);
	
		}
	}
	
	void SelectWeapon(int index)
	{
		soundCubeCreator.SetSelectedPrefab(soundCubePrefabs[index]);
	}
}
