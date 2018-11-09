using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFlow : LifeForm {

  public PaintVectors vectors;
  public FlowParticles verts;
  public Life life;
  public TerrainEngine engine;

  public override void Create(){
    Cycles.Insert(0,verts);
    Cycles.Insert(1,life);
  }

  public override void Bind(){
    life.BindPrimaryForm("_VertBuffer", verts);
    life.BindForm("_VectorBuffer", vectors);
    engine.BindData(life);
  }


}
