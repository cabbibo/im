using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSubject : MonoBehaviour {

  public Transform subject;
  public Transform viewpoint;

  public Vector3 offset;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
    viewpoint.position = subject.position + offset;		
	}
}
