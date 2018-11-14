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

  public Life simulate;
  public Life transfer;

  public override void _Create(){
    Cycles.Insert(Cycles.Count,particles);
    Cycles.Insert(Cycles.Count,setAnchor);
    Cycles.Insert(Cycles.Count,setGlyph);
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

    simulate.BindPrimaryForm("_VertBuffer",particles);

    transfer.BindPrimaryForm("_TransferBuffer",transferVerts);
    transfer.BindForm("_VertBuffer",particles);

    simulate.BindAttribute("_Active","pageActive",book);
    simulate.BindAttribute("_CameraForward","forward",book);
    simulate.BindAttribute("_CameraUp","up",book);
    simulate.BindAttribute("_UrsulaPos","ursulaPos" , book );
    simulate.BindAttribute("_RayOrigin", "RayOrigin",touch);
    simulate.BindAttribute("_RayDirection", "RayDirection",touch);




  }

  public void Set(TextParticles t){
    
    anchor = t;
    print( anchor );
//
    setGlyph.RebindForm("_AnchorBuffer",anchor);
    setAnchor.RebindForm("_AnchorBuffer",anchor);
//
   setAnchor.active = true;
   setAnchor._WhileLiving(1);
   setAnchor.active = false;
//
   setGlyph.active = true;
   setGlyph._WhileLiving(1);
   setGlyph.active = false;

  }

}
