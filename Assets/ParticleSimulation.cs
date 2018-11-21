using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSimulation : LifeForm {

  public Cycle binding;
  public Particles particles;
  public Life setPositions;
  public Life simulation;
  public TransferLifeForm lifeForm;

  public override void _Create(){
    
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,simulation);
    Cycles.Insert(Cycles.Count,setPositions);
    Cycles.Insert(Cycles.Count,lifeForm);
    Cycles.Insert(Cycles.Count,binding);

    DoCreate();

    print("DEGALT");
    _Deactivate();
  
  }


  public override void Bind(){
    simulation.BindPrimaryForm("_VertBuffer",particles);
    setPositions.BindPrimaryForm("_VertBuffer",particles);
  }

  public override void OnBirthed(){
    setPositions.YOLO();
  }

}
