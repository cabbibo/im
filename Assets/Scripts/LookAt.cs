using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

  public Transform target;
  public float slerpSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( target.transform.position - transform.position ), slerpSpeed * Time.deltaTime );
	}
}
