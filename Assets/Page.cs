using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Page : Cycle {

  public float gestateSpeed = 1;
  public float birthSpeed = 1;
  public float deathSpeed = 1;

  public float lockSpeed;
  public float lockStartRadius;
  public float lockEndRadius;

  public Transform subjectTarget;
  public TextParticles text;
  public Frame frame;

  public Page nextPage;
  public Page previousPage;

  public float animationState;
  public Book book;

  public Collider collider;

  private float startTime;
  private float endTime;

  public bool grounded = true;
  // Use this for initialization
  


  public UnityEvent OnDieEvent;
  public UnityEvent OnGestateEvent;
  public UnityEvent OnBirthEvent;
  public UnityEvent OnLiveEvent;

  public override void _Create(){
  
    
    frame.borderLine.enabled = false;
    frame.borderLine.material.SetFloat("_Cutoff" , 1 );
    collider.enabled = true;
//    print("pageCreate");
    DoCreate();
  
  }


  public override void _OnGestate(){
   // print("pageGestated");
    frame.borderLine.enabled = true;
    //OnGestateEvent.Invoke();

    DoGestate();
    OnGestateEvent.Invoke();
  }



  public override void _WhileGestating(float v){
    frame.borderLine.material.SetFloat("_Cutoff" , 1 - .5f * v );
    DoGestating(v);
  }

  public override  void _OnBirth(){
    collider.enabled = false;
    startTime = Time.time;
    DoBirth();
    OnBirthEvent.Invoke();
  }

  public override void _WhileBirthing( float v ){
    frame.borderLine.material.SetFloat("_Cutoff" , .5f - v *.5f);
    DoBirthing(v);
  }

  public override void _OnLive(){
    OnLiveEvent.Invoke();
    DoLive();
  }

  public override  void _OnDie(){
    endTime = Time.time;
    OnDieEvent.Invoke();
    DoDie();
  }

  public override void _WhileDying( float v ){
    if( v < 1 ){ frame.borderLine.material.SetFloat("_Cutoff" ,  v ); }
    DoDying(v);
  }

  public override void _OnDied(){
    frame.borderLine.enabled = false;
    DoDied();
  }


  public override void _Destroy(){
    frame.borderLine.enabled = false;
    //OnDestroyEvent.Invoke();
    DoDestroy();
  }


  
}
