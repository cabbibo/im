using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {

  public RotateXY controls;
  public float speed;
  public Transform camera;
  public Transform subject;
  public Transform subjectTarget;
  public MeshRenderer textMesh;
  public Frame frame;

  public Vector3 offset;
  public bool trackSubject;
  public bool active;

  public float animationState;
  public Book book;

  public Collider collider;

  private float startTime;
  private float endTime;
  // Use this for initialization
  void Awake () {
    Deactivate();

    frame.borderLine.enabled = false;
    
  }
  
  // Update is called once per frame
  void Update () {
    
    if( active ){
      camera.position = Vector3.Lerp( camera.position , transform.position , Time.deltaTime * speed );
      camera.rotation = Quaternion.Lerp( camera.rotation , transform.rotation , Time.deltaTime * speed );
      subject.position = Vector3.Lerp( subject.position , subjectTarget.position , Time.deltaTime * speed );
      book.animationState = Mathf.Lerp( book.animationState , animationState , Time.deltaTime * speed );
      Activating( (Time.time - startTime) / speed );
    }else{
      Deactivating( (Time.time - endTime) / speed );
    }


    if( trackSubject ){
      transform.position = subject.position + offset;   
    }
  }

  public void Activate(){
    active = true;
    collider.enabled = false;
    textMesh.enabled = true;
    startTime = Time.time;
  }

  void Activating( float v ){

    if( v < 1 ){
      frame.borderLine.material.SetFloat("_Cutoff" , .55f - v *.3f);
    }
  }


  void Deactivating( float v ){
    if( v < 1 ){
      frame.borderLine.material.SetFloat("_Cutoff" , .3f + v *.2f);
    }
  }

  void Finalize(){

  }

  public void Deactivate(){

    active = false;
    textMesh.enabled = false;
    endTime = Time.time;
  }

  public void Prepare(){
    frame.borderLine.enabled = true;

  }

  
}
