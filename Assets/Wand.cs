using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour {

  public Transform target;
  public float lerpSpeed;

  public Vector3 position;
  public Vector3 velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    velocity = position;
		transform.position = Vector3.Lerp(transform.position, target.position,lerpSpeed);
    position = transform.position;
    velocity = velocity - position;
	}
}
