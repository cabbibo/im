
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fern : LifeForm {

  public float armLength;
  public Life force;
  public Life constrain;
  public SmoothedHair skeleton;
  public FernVerts verts;
  public override void Create(){


    Lifes.Add( force );
    Lifes.Add( constrain );
    Forms.Add( verts );

    force._Create();
    constrain._Create();
    verts._Create();

    force.BindPrimaryForm("_VertBuffer", verts );
    force.BindForm("_SkeletonBuffer", skeleton );

    force.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", skeleton );
    force.BindAttribute( "_NumHairs" , "numHairs", skeleton );
    force.BindAttribute( "_VertsPerVert" , "vertsPerVert" , verts );

    constrain.BindPrimaryForm("_VertBuffer", verts );
    constrain.BindForm("_SkeletonBuffer", skeleton );

    constrain.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", skeleton );
    constrain.BindAttribute( "_NumHairs" , "numHairs", skeleton );
    constrain.BindAttribute( "_VertsPerVert" , "vertsPerVert" , verts );
    constrain.BindAttribute( "_Length" , "armLength" , this );


  }

  public override void OnGestate(){ 
    verts._OnGestate(); 
  }
  
  public override void WhileLiving(float v){ 
    force.Live(); 
    constrain.Live(); 
  }


}