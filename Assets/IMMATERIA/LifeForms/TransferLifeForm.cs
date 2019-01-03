﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferLifeForm : MeshLifeForm {

  public Life transfer;
  public Body body;
  public bool showBody;
  public Form skeleton;

  [HideInInspector]public Transform cam;
  [HideInInspector]public Vector3 right;
  [HideInInspector]public Vector3 up;
  [HideInInspector]public Vector3 forward;

  // Use this for initialization
  public override void Create(){

    cam = Camera.main.transform;

    Cycles.Insert(Cycles.Count,body);
    Cycles.Insert(Cycles.Count,transfer);


  }

  public override void _Bind(){
    transfer.BindPrimaryForm("_VertBuffer", verts);
    transfer.BindForm("_SkeletonBuffer", skeleton); 

    transfer.BindAttribute("_CameraRight" , "right" , this); 
    transfer.BindAttribute("_CameraUp" , "up" , this); 
    transfer.BindAttribute("_CameraForward" , "forward" , this); 
    Bind();
  }

  public virtual void BindAttributes(){}

  public override void WhileLiving(float v){

    if( active == true ){

      right = cam.right;
      up = cam.up;
      forward = cam.forward;

      if( showBody == true ){
        body.active = true;
      }else{
        body.active = false;
      }

    }

  }

  public override void Activate(){
    showBody = true;
  }

  public override void Deactivate(){
    showBody = false;
  }

}
