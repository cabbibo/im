using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairNoise : Cycle {

  public List<Life> ToBind;

  public TerrainEngine engine;
  public float noiseSize;
  public float falloffRadius;
  public float noiseSpeed;
  public float noiseForce;
  public float normalForce;
  public float dampening;
  public float upForce;
  public float pushForce;
  public float pushRadius;
  public Vector3 player;
  public GameObject subject;

  // Use this for initialization
  public override void Bind () {

    foreach( Life l in ToBind ){
      print(l);
      l.BindAttribute( "_NoiseSpeed" , "noiseSpeed" , this );
      l.BindAttribute( "_NoiseForce" , "noiseForce" , this );
      l.BindAttribute( "_NoiseSize" , "noiseSize" , this );
      l.BindAttribute( "_NormalForce" , "normalForce" , this );
      l.BindAttribute( "_Dampening" , "dampening" , this );
      l.BindAttribute( "_UpForce" , "upForce" , this );
      l.BindAttribute("_Player", "player" , this );
      l.BindAttribute("_FalloffRadius", "falloffRadius" , this );
      l.BindAttribute("_PushForce", "pushForce" , this );
      l.BindAttribute("_PushRadius", "pushRadius" , this );
      engine.BindData(l);
    }
  }

  public void FixedUpdate(){
    player = subject.transform.position;
  }


}
