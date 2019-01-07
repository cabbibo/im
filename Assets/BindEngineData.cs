using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindEngineData : Cycle {

  public TerrainEngine engine;
  public Life simulation;

  public override void Bind(){
  print( this); 
    engine.BindData(simulation);
  }

  
}
