using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBuffer : Cycle{

  public LandscapeScanner[] scanners;

  public int activeScanner;

  public override void _Create(){
    for(int i = 0; i < scanners.Length; i ++ ){
      Cycles.Add(scanners[i]);
    }
    DoCreate();
  }


  public void Scan( Transform t ){
    
    scanners[activeScanner].Scan(t);

    activeScanner ++;
    activeScanner %= scanners.Length;
  }


}
