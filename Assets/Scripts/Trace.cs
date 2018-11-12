using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour {

  public TerrainEngine engine;
  public TouchToRay touch;
  public Transform marker;
  public Vector3 targetPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	  Vector3 p = engine.Trace(touch.RayOrigin,-touch.RayDirection);	
    marker.position = p;
    targetPos = p;
    
    Shader.SetGlobalVector("_WandPosition", p);
	}
  
}
