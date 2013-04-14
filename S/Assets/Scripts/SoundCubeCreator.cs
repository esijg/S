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
		currentCubeOutlineEffect.transform.localPosition = new Vector3(0,0,3.0f);
		currentCubeOutlineEffect.transform.localRotation = Quaternion.identity;
		currentEffectsScript = currentCubeOutlineEffect.GetComponent<CreationCubeFX>();
		currentEffectsScript.StartTransitionIn(this);
		
	}
	
	public void OnCubeEffectTransitionInCompleted()
	{
		currentState = CubeCreationState.Configuring;
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0)){
			BeginCreatingCubeWithSoundID(0);
		}
		
		if ( Input.GetMouseButtonUp(0)){
			Destroy(currentCubeOutlineEffect);
			EndCreatingCube();
		}
	}
	
	void EndCreatingCube()
	{
		if ( currentState == CubeCreationState.EffectsIn )
		{
			currentEffectsScript.AbortEffect();
			currentState = CubeCreationState.Completed;
		}
	}

}
