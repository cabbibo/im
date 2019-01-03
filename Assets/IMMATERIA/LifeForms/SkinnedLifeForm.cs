using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedLifeForm : MeshLifeForm {
  
  public Life skin;

  public Form bones;

	// Use this for initialization
	public override void Create(){
    Cycles.Add(skin);
    Cycles.Add(verts);
    Cycles.Add(triangles);
    Cycles.Add(bones);




	}

  public override void Bind(){
  
    skin.BindPrimaryForm("_VertBuffer",verts);
    skin.BindForm("_BoneBuffer",bones);

  }


}
