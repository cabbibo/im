using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSkinnedParticles : LifeForm {

  public Life life;
  public ParticlesOnSkinnedMesh verts;
  public Form skeleton;

  public override void Create(){
    Cycles.Insert(0,verts);
    Cycles.Insert(1,life);
  }
  public override void Bind(){

    life.BindPrimaryForm("_VertBuffer", verts);
    life.BindForm( "_SkinnedBuffer" , skeleton );

  }

  public  void Set(){
    life.Live();
  }

}
