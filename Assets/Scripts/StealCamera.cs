using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealCamera : MonoBehaviour {

  public RotateXY controls;
  public float speed;
  public Transform camera;
  public Transform viewpoint ;
  public bool active;

	// Use this for initialization
	void Start () {

    active = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
    if( active ){
      camera.position = Vector3.Lerp( camera.position , viewpoint.position , Time.deltaTime * speed );
      camera.rotation = Quaternion.Lerp( camera.rotation , viewpoint.rotation , Time.deltaTime * speed );
    }

	}

  void Steal(){
    controls.enabled = false;
    active = true;
  }

  void Release(){

    controls.enabled = true;
    active = false;
  }

  void OnMouseDown(){

    if( active == false ){
    Steal();
    }else{
      Release();
    }
  }
}
