using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherBall : LifeForm{

  public TerrainEngine engine;
  public TouchToRay touch;
  public MeshVerts baseVerts;
  public MeshTris tris;
  public Life life;
  public Body body;
  public MeshParticles particles;

  public ParticlesOnMeshParticles placedParticles;

  public float[] transformMat;
  public Vector3 position;

  // Use this for initialization
  public override void Create () {

      baseVerts = GetComponent<MeshVerts>();
      tris  = GetComponent<MeshTris>();
      particles  = GetComponent<MeshParticles>();
      life  = GetComponent<Life>();
      body  = GetComponent<Body>();

      Cycles.Insert(0,life);

      Cycles.Insert(1,baseVerts);
      Cycles.Insert(2,body);
      Cycles.Insert(3,placedParticles);
      
  }


  public override void Bind(){

    life.BindPrimaryForm("_VertBuffer", particles);
    life.BindForm("_BaseBuffer", baseVerts);
    life.BindAttribute("_Transform","transformMat",this);

    life.BindAttribute("_HeightMap", "heightMap" , engine);
    life.BindAttribute("_MapSize", "size" , engine);
    life.BindAttribute("_MapHeight", "height" , engine);



    life.BindAttribute("_RayOrigin", "RayOrigin",touch);
    life.BindAttribute("_RayDirection", "RayDirection",touch);


  }


  public override void WhileLiving(float v){
    transformMat = HELP.GetMatrixFloats(transform.localToWorldMatrix);
  }

  public override void OnBirthed(){
    //Set( position );
  }

  
}
