using UnityEngine;
using System.Collections;


//Responsible for handling the initial FX
//and subsequent logic behind creating a sound cube
public class SoundCubeCreator : MonoBehaviour {
	
	enum CubeCreationState{EffectsIn, Configuring, EffectsOut, Completed};
	
	public GameObject cubeOutlineEffectPrefab;
	
	private GameObject currentCubeOutlineEffect;
	private CreationCubeFX currentEffectsScript;
	
	private CubeCreationState currentState = CubeCreationState.Completed;
	
	
	void BeginCreatingCubeWithSoundID(int id)
	{
		EndCreatingCube();
		
		currentState = CubeCreationState.EffectsIn;
		
		currentCubeOutlineEffect = GameObject.Instantiate(cubeOutlineEffectPrefab) as GameObject;
		currentCubeOutlineEffect.transform.parent = transform;
		currentEffectsScript = currentCubeOutlineEffect.GetComponent<CreationCubeFX>();
		currentEffectsScript.StartTransitionIn(this);
		
	}
	
	public void OnCubeEffectTransitionInCompleted()
	{
		currentState = CubeCreationState.Configuring;
	}
	
	void EndCreatingCube()
	{
	}

}
