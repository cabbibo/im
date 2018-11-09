using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMesh : LifeForm{

  public TerrainEngine engine;
  public MeshVerts verts;
  public MeshTris tris;
  public Life life;
  public Body body;
  public HairBasic hairSet;
  public PlaceParticles particlesSet;

  public GameObject sphere;

  public Vector3 position;

	// Use this for initialization
	public override void Create () {

      verts = GetComponent<MeshVerts>();
      tris  = GetComponent<MeshTris>();
      life  = GetComponent<Life>();
      body  = GetComponent<Body>();

      Cycles.Insert(0,life);
      Cycles.Insert(1,body);

	}


  public override void Bind(){

    life.BindPrimaryForm("_VertBuffer", verts);

    life.BindAttribute("_HeightMap", "heightMap" , engine);
    life.BindAttribute("_MapSize", "size" , engine);
    life.BindAttribute("_MapHeight", "height" , engine);
  }

  public override void OnBirthed(){
    Set( position );
  }

  public virtual void Set(Vector3 delta){
//    print(delta);
    //position += delta;
    life.shader.SetVector("_DeltaPos" , delta);
    life.Live();

    particlesSet.Set();
    hairSet.Set();

  }

  void Update(){
    sphere.transform.position = position;
  }
	
}
