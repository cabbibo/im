
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothHair : LifeForm {

  public Life smooth;
  public SmoothedHair smoothed;
  public Hair hair;
  public override void Create(){


    Lifes.Add( smooth );
    Forms.Add( smoothed );

    smooth._Create();
    smoothed._Create();

    smooth.BindPrimaryForm("_VertBuffer", smoothed);
    smooth.BindForm("_SkeletonBuffer", hair );

    smooth.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", hair );
    smooth.BindAttribute( "_NumHairs" , "numHairs", hair );
    smooth.BindAttribute( "_SmoothNumVertsPerHair" , "numVertsPerHair", smoothed );


  }

  public override void OnGestate(){ smoothed._OnGestate(); }
  public override void WhileLiving(float v){ smooth.Live(); }


}