using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandParticles : LifeForm{

  public TransferLifeForm transfer;
  public Particles particles;
  public Life simulation;

  public Story story;
  public Ursula ursula;

  public Wand wand;

  public float radius;

  public TerrainEngine engine;
  public Trace trace;
  public TouchToRay touch;
  public Life transferLife;


  public Vector3 frame1;
  public Vector3 frame2;
  public Vector3 frame3;
  public Vector3 frame4;

	// Use this for initialization
	public override void Create(){
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,simulation);
    Cycles.Insert(Cycles.Count,transfer);
  }	

  public override void Destroy(){
    Cycles.RemoveAt(0);
    Cycles.RemoveAt(0);
    Cycles.RemoveAt(0);

  }
	

  public override void WhileLiving(float v){
       
    frame1 = story.currentPage.frame.bottomLeft;
    frame2 = story.currentPage.frame.bottomRight;
    frame3 = story.currentPage.frame.topLeft;
    frame4 = story.currentPage.frame.topRight;
    
  }

  public override void Bind(){


    simulation.BindPrimaryForm("_VertBuffer",particles);
    simulation.BindAttribute("_Down", "Down",touch);
    simulation.BindAttribute("_CurrentTarget", "currentTargetPosition",story);

    simulation.BindAttribute( "_WandPos", "position" , wand );
    simulation.BindAttribute( "_UrsulaPos", "position" , ursula );
    simulation.BindAttribute( "_UrsulaUp", "up" , ursula );

    simulation.BindAttribute("_Frame1" , "frame1", this);
    simulation.BindAttribute("_Frame2" , "frame2", this);
    simulation.BindAttribute("_Frame3" , "frame3", this);
    simulation.BindAttribute("_Frame4" , "frame4", this);
  
    transferLife.BindAttribute("_Radius","radius", this);
    
    engine.BindData(simulation);

  }

  public void Emit(Vector3 pos){

    simulation.shader.SetFloat("_Emit" , 1 );
    simulation.shader.SetVector("_EmissionPos" , pos );
    simulation._WhileLiving(1);
    simulation.shader.SetFloat("_Emit" , 0 );
  }

}
