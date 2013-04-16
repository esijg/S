using UnityEngine;
using System.Collections;


//Responsible for handling the initial FX
//and subsequent logic behind creating a sound cube
public class SoundCubeCreator : MonoBehaviour {
	
	enum CubeCreationState{EffectsIn, Configuring, EffectsOut, Completed};
	
	public GameObject cubeOutlineEffectPrefab;
	
	private GameObject currentCubeOutlineEffect;
	private CreationCubeFX currentEffectsScript;
	private GameObject soundCube;
	
	private CubeCreationState currentState = CubeCreationState.Completed;
	private float chargeLevel = 0.0f;
	
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
	
	public void OnCubeEffectTransitionInCompleted(GameObject soundCube)
	{
		currentState = CubeCreationState.Configuring;
		this.soundCube = soundCube;
		soundCube.transform.parent = transform;
		
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			BeginCreatingCubeWithSoundID(0);
		}
		if ( !Input.GetMouseButton(0) && currentState != CubeCreationState.Completed)
		{
			if (currentState == CubeCreationState.EffectsIn)
			{
				AbortCreation();
				EndCreatingCube();
			}
			else if ( currentState == CubeCreationState.Configuring)
			{
				EndCreatingCube();
			}
		}
		
		if ( currentState == CubeCreationState.Configuring && chargeLevel <= 0.4f)
		{
			chargeLevel += Time.deltaTime * 1;
       		soundCube.transform.localScale = new Vector3(soundCube.transform.localScale.x+chargeLevel*0.5f, soundCube.transform.localScale.y+chargeLevel*0.5f, soundCube.transform.localScale.z+chargeLevel*0.5f);
		}
		
		
	}
	
	void AbortCreation()
	{
		currentEffectsScript.AbortEffect();
		currentState = CubeCreationState.Completed;
	}
	
	void EndCreatingCube()
	{
		chargeLevel = 0.0f;
		currentState = CubeCreationState.Completed;
		
		if ( soundCube != null )
		{
			soundCube.transform.parent = null;
			soundCube.rigidbody.isKinematic = false;
			soundCube.rigidbody.velocity = transform.forward;
		}
		
		soundCube = null;
	}

}
