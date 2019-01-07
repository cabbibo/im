using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour {

  public MonolithParticles particles;

  void OnMouseDown(){
    particles.emitter = this.transform;
    particles.Emit();
  }
}
