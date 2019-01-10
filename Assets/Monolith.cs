using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour {

  public MonolithParticles particles;
  public ScanBuffer scanner;
  public Marker[] markers;

  void Start(){
    for( int i = 0; i < markers.Length; i++ ){
      markers[i].monolith = this;
    }
  }

  public void OnSelect( Marker m ){
    particles.emitter = m.referenceMonolith.transform;
    particles.Emit();
    scanner.Scan(m.referenceMonolith.transform);
  }

  public void OnDeselect( Marker m ){

  }

}
