using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : Cycle {

  public List<ToggleBool>  buttons;


  public List<LineRenderer> lr;

  public override void Create(){
  foreach( LineRenderer l in lr ){
      l.material.SetFloat("_On" , 0);
      l.enabled = false;
    }

    foreach( ToggleBool b in buttons ){
      b.GetComponent<MeshRenderer>().enabled = false;
    }
  }

  public override void WhileGestating(float v){
    print(v);
    foreach( LineRenderer l in lr ){
      l.material.SetFloat("_On" , v);
    }
  }


  public override void OnGestate(){
  foreach( LineRenderer l in lr ){
      l.material.SetFloat("_On" , 0);
      l.enabled = true;
    }
  }

  public override void OnGestated(){
    foreach( ToggleBool b in buttons ){
      b.GetComponent<MeshRenderer>().enabled = true;
    }
  }

    public override void WhileDying(float v){
    print(v);
    foreach( LineRenderer l in lr ){
      l.material.SetFloat("_On" , 1-v);
    }

  }

  public override void OnDie(){
    foreach( ToggleBool b in buttons ){
      b.GetComponent<MeshRenderer>().enabled = false;
    }

  }

  public override void OnDied(){
    foreach( LineRenderer l in lr ){
      l.enabled = false;
    }

  }

  


}
