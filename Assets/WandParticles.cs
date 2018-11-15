using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandParticles : LifeForm{

  public TransferLifeForm transfer;
  public Particles particles;
  public Life simulation;

  public Book book;

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
       
    frame1 = book.pages[book.currentPage].frame.bottomLeft;
    frame2 = book.pages[book.currentPage].frame.bottomRight;
    frame3 = book.pages[book.currentPage].frame.topLeft;
    frame4 = book.pages[book.currentPage].frame.topRight;
    
  }

  public override void Bind(){


    simulation.BindPrimaryForm("_VertBuffer",particles);
    simulation.BindAttribute("_Down", "Down",touch);
    simulation.BindAttribute("_CurrentTarget", "currentTargetPosition",book);

    simulation.BindAttribute( "_WandPos", "position" , wand );

    simulation.BindAttribute("_Frame1" , "frame1", this);
    simulation.BindAttribute("_Frame2" , "frame2", this);
    simulation.BindAttribute("_Frame3" , "frame3", this);
    simulation.BindAttribute("_Frame4" , "frame4", this);
  
    transferLife.BindAttribute("_Radius","radius", this);
    
    engine.BindData(simulation);

  }

}
