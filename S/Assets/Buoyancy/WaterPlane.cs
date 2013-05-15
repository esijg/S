using UnityEngine;
using System.Collections;

public class WaterPlane : MonoBehaviour {
	
	public float waterDensity = 1f;//
	public float waterDrag = 1.5f;
	public float waterAngularDrag = 1f;
	public Texture2D currents;
	public float currentStrength = 2f;//
	
	private static WaterPlane s_Instance = null;
    public static WaterPlane instance {
        get {
            if (s_Instance == null) {
                s_Instance =  FindObjectOfType(typeof (WaterPlane)) as WaterPlane;
                if (s_Instance == null)
                	Debug.Log("There's no instance of WaterPlane in the scene");
            }
            return s_Instance;
        }
    }
    
    void OnApplicationQuit() {
        s_Instance = null;
    }
}
