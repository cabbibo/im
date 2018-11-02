using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairNoise : Cycle {

  public Life toBind;

    public float noiseSize;
  public float noiseSpeed;
  public float noiseForce;
  public float normalForce;
  public float dampening;
  public float upForce;
  public float pushForce;
  public float pushRadius;
  public Vector3 player;
  public GameObject p;

  // Use this for initialization
  public override void Bind () {
    toBind.BindAttribute( "_NoiseSpeed" , "noiseSpeed" , this );
    toBind.BindAttribute( "_NoiseForce" , "noiseForce" , this );
    toBind.BindAttribute( "_NoiseSize" , "noiseSize" , this );
    toBind.BindAttribute( "_NormalForce" , "normalForce" , this );
    toBind.BindAttribute( "_Dampening" , "dampening" , this );
    toBind.BindAttribute( "_UpForce" , "upForce" , this );
    toBind.BindAttribute("_Player", "player" , this );
    toBind.BindAttribute("_PushForce", "pushForce" , this );
    toBind.BindAttribute("_PushRadius", "pushRadius" , this );
  }

  public void FixedUpdate(){
    player = p.transform.position;
  }


}
