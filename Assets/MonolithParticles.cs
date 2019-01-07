using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithParticles : LifeForm{

  public Book book;
  public TerrainEngine engine;
  public Particles particles;
  public Form transferVerts;
  public TouchToRay touch;
  public Transform emitter;
  public Vector3 emitterPos;

  public TransferLifeForm transfer;
  public Body body;

  public Life simulate;
  public float radius;
  public float scale;
  public float emitting;


  public void HideShowParticles(bool val){
    body.active = val;
  }

  public override void _Create(){
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,simulate);
    Cycles.Insert(Cycles.Count,transfer);
    DoCreate();
  }

  public override void Bind(){


    simulate.BindPrimaryForm("_VertBuffer",particles);
    simulate.BindAttribute("_UrsulaPos","position" , book.ursula );
  
    simulate.BindAttribute("_RayOrigin", "RayOrigin",touch);
    simulate.BindAttribute("_RayDirection", "RayDirection",touch);
    simulate.BindAttribute("_Emitting", "emitting",this);
    simulate.BindAttribute("_EmitterPosition", "emitterPos",this);

    engine.BindData(simulate);

  }

  public void Emit(){
    emitting = 1 - emitting;
    emitterPos = emitter.position;
  }

  


}
