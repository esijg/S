using UnityEngine;
using System.Collections;

//handles all the logic associated with the cube creation vfx
public class CreationCubeFX : MonoBehaviour 
{
	public Transform leftFaceOutTransform;
	public Transform rightFaceOutTransform;
	public float fadeInDuration = 0.1f;
	public float transitionDuration = 1.0f;
	public float degreesPerFrame = 0.1f;
	public Vector3 spinAxis = Vector3.one;
	public GameObject cubeProxy;
	
	public Material[] materials;
	public GameObject[] planes;
	public Transform leftHalf, rightHalf;
	
	private SoundCubeCreator callingObject;
	
	//fade in vars	
	private bool fadingIn = false;	
	private Color startingColor;
	private Color endingColor;
	private float startTime = 0.0f;
	
	//transition vars
	private bool transitioningIn = false;
	private float transitionStartTime;
	
	//spinning vars
	private bool spinning = false;
	
	public void SetCubePrefab(GameObject prefab)
	{
		Vector3 pos = cubeProxy.transform.position;
		Vector3 localScale = cubeProxy.transform.localScale;
		Quaternion rot = cubeProxy.transform.rotation;
		
		Destroy(cubeProxy);
		
		cubeProxy = GameObject.Instantiate(prefab) as GameObject;
		cubeProxy.rigidbody.isKinematic = true;
		cubeProxy.collider.enabled = false;
		cubeProxy.renderer.enabled = false;
		cubeProxy.transform.position = pos;
		cubeProxy.transform.localScale = localScale;
		cubeProxy.transform.rotation = rot;
		
		cubeProxy.transform.parent = transform;
		
	}
	
	void Start()
	{
		int index = 0;
		cubeProxy.renderer.enabled = false;
		foreach(Transform t in leftHalf){
			if ( t.gameObject.name=="Plane") planes[index++] = t.gameObject;
		}
		
		foreach(Transform t in rightHalf){
			if ( t.gameObject.name=="Plane") planes[index++] = t.gameObject;
		}
		
		
		startingColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		
		foreach (Transform t in leftHalf.transform)
		{
			if ( t.gameObject.GetComponent<Collider>() != null) t.collider.enabled = false;
		}
		foreach (Transform t in rightHalf.transform)
		{
			if ( t.gameObject.GetComponent<Collider>() != null) t.collider.enabled = false;
		}
		
		foreach(GameObject o in planes)
		{
			o.renderer.material.color = startingColor;
		}
	}
	
	public void StartTransitionIn(SoundCubeCreator caller)
	{
		callingObject = caller;	
		transitioningIn = true;
		transitionStartTime = Time.time;
		
		
	}
	
	public void FadeInFacesWithMaterialID(int id)
	{
		startingColor = new Color(materials[id].color.r, materials[id].color.g, materials[id].color.b, 0.0f);
		endingColor = new Color(materials[id].color.r, materials[id].color.g, materials[id].color.b, 1.0f);
		
		foreach(GameObject o in planes)
		{
			o.renderer.material.color = startingColor;
		}
		
		fadingIn = true;
		startTime = Time.time;
	}
	
	void Update()
	{
		
		if ( transitioningIn)
		{
			leftHalf.transform.localPosition = Vector3.Lerp(leftFaceOutTransform.transform.localPosition, Vector3.zero, (Time.time - transitionStartTime)/transitionDuration);
			leftHalf.transform.localScale = Vector3.Lerp(leftFaceOutTransform.transform.localScale, Vector3.one, (Time.time - transitionStartTime)/transitionDuration);
			leftHalf.transform.localRotation = Quaternion.Lerp(leftFaceOutTransform.transform.localRotation, Quaternion.identity, (Time.time - transitionStartTime)/transitionDuration);
			
			rightHalf.transform.localPosition = Vector3.Lerp(rightFaceOutTransform.transform.localPosition, Vector3.zero, (Time.time - transitionStartTime)/transitionDuration);
			rightHalf.transform.localScale = Vector3.Lerp(rightFaceOutTransform.transform.localScale, Vector3.one, (Time.time - transitionStartTime)/transitionDuration);
			rightHalf.transform.localRotation = Quaternion.Lerp(rightFaceOutTransform.transform.localRotation, Quaternion.identity, (Time.time - transitionStartTime)/transitionDuration);
			
			if ( Vector3.Distance(rightHalf.transform.localPosition, Vector3.zero) < Vector3.kEpsilon)
			{
				FadeInFacesWithMaterialID(0);
				transitioningIn = false;
			}
		}
		
		if (spinning)
		{
			gameObject.transform.RotateAround(spinAxis,degreesPerFrame);
		}
		
		
		if ( fadingIn )
		{
			bool completeFlag = false;
			
			foreach(GameObject o in planes)
			{
				o.renderer.material.color = Color.Lerp(startingColor, endingColor, (Time.time - startTime)/fadeInDuration);
				if ( o.renderer.material.color.a >= 1.0f ) completeFlag = true;
			}
			
			if (completeFlag)
			{
				fadingIn = false;
				cubeProxy.transform.parent = null;
				cubeProxy.renderer.enabled = true;
				cubeProxy.collider.enabled = true;
				callingObject.OnCubeEffectTransitionInCompleted(cubeProxy);

				DestroyThis();

			}
		}
	}
	
	public void AbortEffect()
	{
		if ( Vector3.Distance(rightHalf.transform.localPosition, Vector3.zero) > 4.0f ){
			DestroyThis();
			return;
		}
		
		foreach (Transform t in leftHalf.transform)
		{
			if ( t.gameObject.GetComponent<Collider>() != null) t.collider.enabled = true;
		}
		foreach (Transform t in rightHalf.transform)
		{
			if ( t.gameObject.GetComponent<Collider>() != null) t.collider.enabled = true;
		}
		
		transitioningIn = false;
		transform.parent = null;
		leftHalf.rigidbody.isKinematic = false;
		rightHalf.rigidbody.isKinematic = false;
		Invoke("DestroyThis", 1.0f);
	}
	
	void DestroyThis()
	{
		Destroy(this.gameObject);
	}
}
