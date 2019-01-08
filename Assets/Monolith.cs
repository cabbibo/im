using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour {

  public MonolithParticles particles;
  public ScanBuffer scanner;

  void OnMouseDown(){
    particles.emitter = this.transform;
    particles.Emit();
    scanner.Scan(this.transform);
  }
}
