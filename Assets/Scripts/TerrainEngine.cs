﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class TerrainEngine : LifeForm {

  public float size;
  public float height;
  public float tileSize;
  public Texture2D heightMap;
  public Transform player;
  public int numTiles;
  public GameObject tilePrefab;

  public Transform[] planes;
  public SetMesh[] planeLifes;

  public MeshFilter plane1;
  
  private float hT;
  private float t;

  public override void Destroy(){

    for( int i = 0; i < planes.Length; i++ ){

      if( planes[i] ){
        DestroyImmediate(planes[i].gameObject);
        Cycles.RemoveAt(1);
      }
    }

  }

  public override void Create(){
    planeLifes = new SetMesh[numTiles * numTiles];
    planes     = new Transform[numTiles * numTiles];

    t = (float)numTiles;
    hT = t /2;
    for( int i = 0; i < numTiles; i++ ){
      for( int j = 0; j < numTiles; j++ ){

        GameObject plane = Instantiate( tilePrefab );
  
        int id = i * numTiles + j;
        planes[id] = plane.transform;
  
        float x = (float)i-hT;
        float y = (float)j-hT;


  
        planeLifes[id] = plane.gameObject.GetComponent<SetMesh>();
        planeLifes[id].position = new Vector3( x *tileSize, 0 , y*tileSize);

        Cycles.Insert( id +2, planeLifes[id]);
      }
    }
  }

	// Use this for initialization
	public override void WhileLiving (float l) {

    plane1.sharedMesh.bounds = new Bounds (Vector3.zero,Vector3.one * 100000);

    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);


    Vector3 oPos;
    for( int i = 0; i < planeLifes.Length; i++ ){

      oPos = planeLifes[i].position;

      if( player.position.x - planeLifes[i].position.x < -hT * tileSize ){        
        planeLifes[i].position -= Vector3.right * t* tileSize ;
      }

      if( player.position.x - planeLifes[i].position.x > hT* tileSize  ){
        planeLifes[i].position += Vector3.right * t* tileSize ;
      }


      if( player.position.z - planeLifes[i].position.z < -hT * tileSize ){
        planeLifes[i].position -= Vector3.forward * t* tileSize ;
      }

      if( player.position.z - planeLifes[i].position.z > hT * tileSize  ){
        planeLifes[i].position += Vector3.forward * t* tileSize ;
      }

      if( oPos != planeLifes[i].position ){
        planeLifes[i].Set( (planeLifes[i].position-oPos) );
      }
    }
		
	}

  public float SampleHeight( Vector2 v ){
    float posX = (v.x-.5f)* size;
    float posZ = (v.y-.5f)* size;
    Color c =  heightMap.GetPixelBilinear(posX , posZ);
    return c.r * height;
  }


  public float SampleHeight( Vector3 v ){
    float posX = (v.x-.5f)* size;
    float posZ = (v.z-.5f)* size;
    Color c =  heightMap.GetPixelBilinear(posX , posZ);
    return c.r * height;
  }

  public Vector3 Trace( Vector3 ro , Vector3 rd ){

    for( int i = 0; i < 1000; i++ ){

      Vector3 pos = ro+ rd * i * 1;
      float h = SampleHeight( pos );
      if( pos.y < h ){
        return pos;
//        break;
      }

    }

    return ro + rd * 100;



  }


  public void BindData(Life life){

    life.BindAttribute("_HeightMap", "heightMap" , this );
    life.BindAttribute("_MapSize"  , "size" , this );
    life.BindAttribute("_MapHeight", "height" , this );

  }


}