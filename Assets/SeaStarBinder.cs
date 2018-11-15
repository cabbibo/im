using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaStarBinder : Cycle {

  public Life setPosition;
  public Life simulate;
  public Life transfer;
  

  public Ursula ursula;
  public Trace trace;
  public TouchToRay touch;
  public Wand wand;
  public float radius;

  public float startWidth;
  public float startDepth;
  public Vector3 centerPosition;

  // Use this for initialization
  public override void Bind () {

    centerPosition = transform.position;

    setPosition.BindAttribute( "_StartWidth" , "startWidth" , this );
    setPosition.BindAttribute( "_StartDepth" , "startDepth" , this );
    
    simulate.BindAttribute( "_UrsulaPos" , "position" , ursula );
    simulate.BindAttribute("_WandPos", "position" , wand );
    simulate.BindAttribute("_RO", "RayOrigin" , touch);
    simulate.BindAttribute("_RD", "RayDirection" , touch );
    simulate.BindAttribute("_CenterPos1" , "centerPosition" , this );

    transfer.BindAttribute("_Radius","radius",this);

  }


  void FixedUpdate(){
    centerPosition = transform.position;
  }



}
