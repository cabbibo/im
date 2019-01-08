using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeScanner : LifeForm{

  public TerrainEngine engine;
  public Particles particles; 
  public Transform scanner;
  public Vector3 scannerPos;

  public TransferLifeForm transfer;



  public Form scanVerts;
  public Life scan;

  public float scanStartTime;
  public float scanTime;


  public override void WhileLiving(float v){

    scanTime = Time.time - scanStartTime;
  }
  public void Scan(Transform t){
    scanner = t;
    scannerPos = scanner.position;
    scanStartTime = Time.time;
  }

  public void HideShowParticles(bool val){
    transfer.showBody = val;
  }

  public override void _Create(){
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,scan);
    Cycles.Insert(Cycles.Count,transfer);
    DoCreate();
  }

  public override void Bind(){


    scan.BindPrimaryForm("_VertBuffer",particles);
  
    scan.BindAttribute("_ScanPosition", "scannerPos",this);
    scan.BindAttribute("_ScanTime", "scanTime",this);

    engine.BindData(scan);

  }




}