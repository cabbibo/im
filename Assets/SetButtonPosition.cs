using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SetButtonPosition : MonoBehaviour {

  public Frame frame;
  public float size;
	
	// Update is called once per frame
	void Update () {
		transform.position = frame.bottomRight - size * frame.right * transform.localScale.x + frame.up * transform.localScale.y ;
	}
}
