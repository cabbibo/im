using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Page : Cycle {

  public float gestateSpeed = 1;
  public float birthSpeed = 1;
  public float deathSpeed = 1;

  public Transform subjectTarget;
  public TextParticles text;
  public Frame frame;

  public float animationState;
  public Book book;

  public Collider collider;

  private float startTime;
  private float endTime;
  // Use this for initialization
  


  public UnityEvent OnDieEvent;

  public override void _Create(){
  
    
    frame.borderLine.enabled = false;
    frame.borderLine.material.SetFloat("_Cutoff" , .65f);
    print("pageCreate");
    DoCreate();
  
  }


  public override void _OnGestate(){
    frame.borderLine.enabled = true;
    DoGestate();
  }

  public override void _WhileGestating(float v){
    frame.borderLine.material.SetFloat("_Cutoff" , .65f - v *.2f);
    DoGestating(v);
  }

  public override  void _OnBirth(){
    collider.enabled = false;
    startTime = Time.time;
    DoBirth();
  }

  public override void _WhileBirthing( float v ){
    frame.borderLine.material.SetFloat("_Cutoff" , .55f - v *.3f);
    DoBirthing(v);
  }


  public override  void _OnDie(){
    endTime = Time.time;
    OnDieEvent.Invoke();
    DoDie();
  }

  public override void _WhileDying( float v ){
    if( v < 1 ){ frame.borderLine.material.SetFloat("_Cutoff" , .3f + v *.2f); }
    DoDying(v);
  }


  public override void _Destroy(){
    frame.borderLine.enabled = false;
    //OnDestroyEvent.Invoke();
    DoDestroy();
  }


  
}
