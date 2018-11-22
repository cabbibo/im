using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTextParticles : LifeForm{

  public Book book;
  public Particles particles;
  public TextParticles anchor;
  public Form transferVerts;
  public TouchToRay touch;

  public Body body;

  public Life setAnchor;
  public Life setGlyph;
  public Life setPage;

  public Life simulate;
  public Life transfer;
  private float pageStart;

  public float radius;

  public float scale;

  public void HideShowParticles(bool val){
    body.active = val;
  }

  public override void _Create(){
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,setAnchor);
    Cycles.Insert(Cycles.Count,setGlyph);
    Cycles.Insert(Cycles.Count,setPage);
    Cycles.Insert(Cycles.Count,simulate);
    Cycles.Insert(Cycles.Count,transfer);
    Cycles.Insert(Cycles.Count,body);
    DoCreate();
  }

  public override void Bind(){

    setGlyph.BindPrimaryForm("_TransferBuffer",transferVerts);
    setGlyph.BindForm("_AnchorBuffer",anchor);

    setAnchor.BindPrimaryForm("_VertBuffer",particles);
    setAnchor.BindForm("_AnchorBuffer",anchor);

    setPage.BindPrimaryForm("_VertBuffer",particles);

    simulate.BindPrimaryForm("_VertBuffer",particles);

    transfer.BindPrimaryForm("_TransferBuffer",transferVerts);
    transfer.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Radius","radius",this);//.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Scale","scale",this);//.BindForm("_VertBuffer",particles);


    simulate.BindAttribute("_Active","pageActive",book);
    
    simulate.BindAttribute("_Up","up",book.ursula);
    simulate.BindAttribute("_CameraForward","forward",book);
    simulate.BindAttribute("_CameraUp","up",book);
    simulate.BindAttribute("_PageAlive","pageAlive",book);
    simulate.BindAttribute("_UrsulaPos","position" , book.ursula );
    
    simulate.BindAttribute("_Fade","fade" , book );

    simulate.BindAttribute("_RayOrigin", "RayOrigin",touch);
    simulate.BindAttribute("_RayDirection", "RayDirection",touch);




  }

  public void Set(TextParticles t){
    
    anchor = t;
    scale = t.scale;

    setGlyph.RebindForm("_AnchorBuffer",anchor);
    setAnchor.RebindForm("_AnchorBuffer",anchor);

    setAnchor.YOLO();
    setGlyph.YOLO();
  }

  public void PageStart(){
    setPage.YOLO();
  }


}
