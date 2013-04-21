using UnityEngine;
using System.Collections;


//Responsible for handling the initial FX
//and subsequent logic behind creating a sound cube
public class SoundCubeCreator : MonoBehaviour {
	
	enum CubeCreationState{EffectsIn, Configuring, EffectsOut, Completed};
	
	public GameObject cubeOutlineEffectPrefab;
	
	public float chargeSpeed = 0.5f;
	private GameObject currentCubeOutlineEffect;
	private CreationCubeFX currentEffectsScript;
	private GameObject soundCube;
	
	private CubeCreationState currentState = CubeCreationState.Completed;
	private float chargeLevel = 0.0f;
	
	private GameObject selectedPrefab;
	
	
	public void SetSelectedPrefab(GameObject prefab)
	{
		selectedPrefab = prefab;
	}
	
	void BeginCreatingCubeWithSoundID(int id)
	{
		EndCreatingCube(false);
		
		currentState = CubeCreationState.EffectsIn;
		
		currentCubeOutlineEffect = GameObject.Instantiate(cubeOutlineEffectPrefab) as GameObject;
		currentCubeOutlineEffect.transform.parent = transform;
		currentCubeOutlineEffect.transform.localPosition = new Vector3(0,0,3.0f);
		currentCubeOutlineEffect.transform.localRotation = Quaternion.identity;
		currentEffectsScript = currentCubeOutlineEffect.GetComponent<CreationCubeFX>();
		currentEffectsScript.SetCubePrefab(selectedPrefab);
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
		if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
		{
			BeginCreatingCubeWithSoundID(0);
		}
		if ( !Input.GetMouseButton(0) && currentState != CubeCreationState.Completed)
		{
			if (currentState == CubeCreationState.EffectsIn)
			{
				AbortCreation();
				EndCreatingCube(false);
			}
			else if ( currentState == CubeCreationState.Configuring)
			{
				EndCreatingCube(false);
			}
		}
		else if (Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))
		{
			if (currentState == CubeCreationState.EffectsIn)
			{
				AbortCreation();
				EndCreatingCube(true);
			}
			else if ( currentState == CubeCreationState.Configuring)
			{
				EndCreatingCube(true);
			}
		}
		
		if ( currentState == CubeCreationState.Configuring && chargeLevel <= 0.19f)
		{
			chargeLevel += Time.deltaTime * chargeSpeed;
			soundCube.audio.volume = chargeLevel*4;
       		soundCube.transform.localScale = new Vector3(soundCube.transform.localScale.x+chargeLevel*0.5f, soundCube.transform.localScale.y+chargeLevel*0.5f, soundCube.transform.localScale.z+chargeLevel*0.5f);
		}
		
		
	}
	
	void AbortCreation()
	{
		currentEffectsScript.AbortEffect();
		currentState = CubeCreationState.Completed;
	}
	
	void EndCreatingCube(bool thrown)
	{
		chargeLevel = 0.0f;
		currentState = CubeCreationState.Completed;
		
		if ( soundCube != null )
		{
			Debug.Log("End creation");
			soundCube.transform.parent = null;
			soundCube.audio.Play();
			soundCube.rigidbody.isKinematic = false;
			
			
			if (thrown)
			{
				soundCube.rigidbody.velocity = transform.forward*10;
			}
		}
		
		soundCube = null;
	}

}
