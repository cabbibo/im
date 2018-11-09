using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceParticles : LifeForm {

  public Life life;
  public ParticlesOnDynamicMesh verts;
  public MeshVerts skeleton;

  public override void Create(){
    Cycles.Insert(0,verts);
    Cycles.Insert(1,life);
  }
  public override void Bind(){

    life.BindPrimaryForm("_VertBuffer", verts);
    life.BindForm( "_SkeletonBuffer" , skeleton );

  }

  public  void Set(){
    life.Live();
  }

}
