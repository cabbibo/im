using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTraceTarget : MonoBehaviour {

  public Trace trace;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = trace.targetPos;
	}
}
