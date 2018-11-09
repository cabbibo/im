using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RotateCamera : MonoBehaviour {

  public float height;
  public ForceCamera cam;

  public Vector3 target;
  
  public Transform subject;
  public float rotateSizeX;
  public float rotateSizeY;

  public float angle;
  public float radius;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    // Read the user input
    var x = CrossPlatformInputManager.GetAxis("Mouse X");
    //var y = CrossPlatformInputManager.GetAxis("Mouse Y");

   // print( x );
    angle += rotateSizeX * x;

    target = subject.position + new Vector3(0,1,0) * height;
    target -= radius * Vector3.left * Mathf.Sin( angle  );
    target += radius * Vector3.forward * Mathf.Cos( angle );

    cam.target = target;	
	}
}
