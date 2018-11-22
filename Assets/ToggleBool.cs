using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class boolEvent : UnityEvent<bool>{}

public class ToggleBool : Cycle {


  public LineRenderer lr;

  public boolEvent toggle;

  public bool toggled;

	// Use this for initialization
	void Start () {

    lr = GetComponent<LineRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseOver(){
    GetComponent<MeshRenderer>().material.SetFloat("_Hovered" , 1 );
  }

  void OnMouseOut(){
    GetComponent<MeshRenderer>().material.SetFloat("_Hovered" , 0 );
  }

  void OnMouseDown(){
    toggle.Invoke(toggled);
    toggled = !toggled;
  }

  public override void OnDie(){

    toggle.Invoke(true);

  }
}
