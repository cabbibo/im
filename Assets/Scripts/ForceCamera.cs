using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCamera : MonoBehaviour {

  public Vector3 target;
  public Transform subject;

  public float speed;
  public bool selfSet;
  private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
	 
  	transform.LookAt( subject );
	}

  void FixedUpdate(){
   // if (selfSet){ target = subject.transform.position + new Vector3(10,10,10); }
    Vector3 f = target - transform.position;
    rb.AddForce( f * speed);
  }


}
