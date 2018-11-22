using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

  public float speed;

  public float slowSpeed;
  public float fastSpeed;

	// Use this for initialization
	void Start () {
		
	}

	public void Toggle( bool val ){

		if(val){
			speed = slowSpeed;
		}else{
			speed = fastSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {
	 transform.Rotate(Vector3.up,Time.deltaTime * speed, Space.World);	

	}
}
