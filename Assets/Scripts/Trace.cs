using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour {

  public TerrainEngine engine;
  public Book book;
  public TouchToRay touch;
  public Transform marker;
  public Vector3 targetPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
    Vector3 p; 
    
    //if( book.pageActive > .5f ){
    //  p = touch.ray.origin + 4*touch.ray.direction;
    //}else{
      p = engine.Trace(touch.ray.origin,touch.ray.direction);	
    //}

    targetPos = p;
    Shader.SetGlobalVector("_WandPosition", p);
	}
  
}
