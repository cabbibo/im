using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexParticleTransferVerts: Form {


  public Form particles;
  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    count = particles.count * 7;
  }

}



