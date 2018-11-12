using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintVectors : Form {

  public TerrainEngine engine;
  public Material lineDebugMaterial;
  public int width;

  /*
  float3 pos
  float2 dir
  float2 pixelPos
  float debug;
*/


  public override void SetStructSize(){ structSize = 12; }
  public override void SetCount(){

    width = engine.heightMap.width;

    count = engine.heightMap.width * engine.heightMap.width;
  }

  public override void Embody(){

    int index = 0;

    float[] values = new float[count*structSize];
    for( int i = 0; i < count; i ++ ){

      int x = i % engine.heightMap.width;
      int y = i / engine.heightMap.width;

      Color c =  engine.heightMap.GetPixelBilinear((float)x / (float)engine.heightMap.width , (float)y / (float)engine.heightMap.width);


      values[ index ++ ] = ((float)x/(float)engine.heightMap.width) / engine.size;
      values[ index ++ ] = c.r * engine.height;
      values[ index ++ ] = ((float)y/(float)engine.heightMap.width) / engine.size;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = c.g * 2 -1;
      values[ index ++ ] = 0;
      values[ index ++ ] = c.b* 2 -1;

      values[ index ++ ] = x;
      values[ index ++ ] = y;

      values[ index ++ ] = c.a;
    
    }

    SetData( values );

  }


  public override void WhileDebug(){
    
    lineDebugMaterial.SetPass(0);
    lineDebugMaterial.SetBuffer("_VertBuffer", _buffer);
    lineDebugMaterial.SetInt("_Count",count);
    Graphics.DrawProcedural(MeshTopology.Lines, count  * 2 );

    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    Graphics.DrawProcedural(MeshTopology.Triangles, count * 3 * 2 );

  }

}
