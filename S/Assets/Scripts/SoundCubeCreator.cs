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
		soundCube.audio.pitch = 0.0f;
		chargeLevel = 0.0f;
		
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
		
		if ( currentState == CubeCreationState.Configuring && chargeLevel <= 4.0f)
		{
			if (!Input.GetMouseButton(0))EndCreatingCube(false);
			chargeLevel+=Time.deltaTime;
			if (soundCube != null)soundCube.audio.pitch= 0.0f + chargeLevel;
			if (soundCube != null)soundCube.collider.enabled = true;
       		if (soundCube != null)soundCube.transform.localScale = new Vector3(soundCube.transform.localScale.x+ Time.deltaTime*0.25f, soundCube.transform.localScale.y+ Time.deltaTime*0.25f, soundCube.transform.localScale.z+ Time.deltaTime*0.25f);
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
			soundCube.GetComponent<SoundCubeID>().Throw();
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
