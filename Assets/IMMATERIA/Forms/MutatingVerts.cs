using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatingVerts : Form {

  public Mesh mesh;
  
  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector3 tang;
    public Vector2 uv;
    public float debug;
  };

  public override void SetStructSize(){ structSize = 12; }

  public override void SetCount(){ 
    count = mesh.vertices.Length;
  }

public override void Embody(){
  Mutate();
}

public override void _WhileLiving(float v){
  DoLiving(v);
  Mutate();
}

  public void Mutate(){

    Vector3[] verts = mesh.vertices;
    Vector2[] uvs = mesh.uv;
    Vector3[] nors = mesh.normals;
    Vector4[] tangs = mesh.tangents;

    int index = 0;


   //Vector3 pos;
   //Vector3 uv;
   //Vector3 nor;
   //int baseTri;

    float[] values = new float[count*structSize];
    for( int i = 0; i < count; i ++ ){
      values[ index ++ ] = verts[i].x;
      values[ index ++ ] = verts[i].y;
      values[ index ++ ] = verts[i].z;

      values[ index ++ ] = nors[i].x;
      values[ index ++ ] = nors[i].y;
      values[ index ++ ] = nors[i].z;

      if( tangs.Length > i ){
        values[ index ++ ] = tangs[i].x;
        values[ index ++ ] = tangs[i].y;
        values[ index ++ ] = tangs[i].z;
      }else{
        Vector3 tang = Vector3.Cross( nors[i] , Vector3.forward );
        values[ index ++ ] = tang.x;
        values[ index ++ ] = tang.y;
        values[ index ++ ] = tang.z;

      }

      values[ index ++ ] = uvs[i].x;
      values[ index ++ ] = uvs[i].y;

      values[ index ++ ] = (float)i/(float)count;
    }
    
    SetData( values );
  }
}