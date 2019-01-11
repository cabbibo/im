using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainEngine : LifeForm {

  public float size;
  public float height;
  public float tileSize;
  public Texture2D heightMap;
  public Transform player;
  public int numTiles;
  public GameObject tilePrefab;

  public int traceSteps;
  public float traceDist;

  public HairNoise hairController;
  public Transform[] planes;
  public Transform[] waters;
  public SetMesh[] planeLifes;

  public MeshFilter plane1;
  
  private float hT;
  private float t;

  public override void Destroy(){

    for( int i = 0; i < planes.Length; i++ ){

      if( planes[i] ){
        DestroyImmediate(planes[i].gameObject);
        Cycles.RemoveAt(0);
      }
    }


    // removes our hair noise
    //Cycles.RemoveAt(0);

    hairController.ToBind.Clear();
  }

  public override void Create(){


    Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);

    planeLifes = new SetMesh[numTiles * numTiles];
    planes     = new Transform[numTiles * numTiles];
    waters     = new Transform[numTiles * numTiles];

    t = (float)numTiles;
    hT = t /2;
    for( int i = 0; i < numTiles; i++ ){
      for( int j = 0; j < numTiles; j++ ){

        GameObject plane = Instantiate( tilePrefab );
  
        int id = i * numTiles + j;
        planes[id] = plane.transform;
  
        float x = (float)i-hT;
        float y = (float)j-hT;

        plane.transform.parent = transform;



  
        planeLifes[id] = plane.gameObject.GetComponent<SetMesh>();
        planeLifes[id].position = new Vector3( x *tileSize, 0 , y*tileSize);

         waters[id] = planeLifes[id].water;//.GetComponent<Water>();
         waters[id].position = planeLifes[id].position;

        Cycles.Insert( Cycles.Count , planeLifes[id]);
        hairController.ToBind.Add(planeLifes[id].hairSet.HairCollision);
      }
    }

    // /Cycles.Insert( Cycles.Count , hairController );
  }

	// Use this for initialization
	public override void WhileLiving (float l) {

    plane1.sharedMesh.bounds = new Bounds (Vector3.zero,Vector3.one * 100000);


   Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);


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

      waters[i].position = new Vector3( planeLifes[i].position.x , planeLifes[i].waterHeight,planeLifes[i].position.z);
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

    for( int i = 0; i < traceSteps; i++ ){

      Vector3 pos = ro +rd * i *traceDist;
      float h = SampleHeight( pos );

      if( pos.y < h ){

       // print( h);
        return pos;
//        break;
      }

    }

    return ro + rd * 40;

  }


  public Vector3 Trace( Vector3 ro , Vector3 rd , out bool hit ){

    hit = false;
    for( int i = 0; i < traceSteps; i++ ){

      Vector3 pos = ro +rd * i *traceDist;
      float h = SampleHeight( pos );

      if( pos.y < h ){
        hit = true;
       // print( h);
        return pos;
//        break;
      }

    }

    return ro + rd * 40;

  }

  public void BindData(Life life){

    life.BindAttribute("_HeightMap", "heightMap" , this );
    life.BindAttribute("_MapSize"  , "size" , this );
    life.BindAttribute("_MapHeight", "height" , this );

  }


}
