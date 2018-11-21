using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticles : Particles {

  public MeshVerts baseVerts;
  
  public override void SetCount(){
    count = baseVerts.count;
  }


}
