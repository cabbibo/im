using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VectorPainter : LifeForm {


  public PaintVectors vectors;
  public PaintVectorIndexForm indices;
  public Life life;
  public Trace trace;
  public Body body;
  public TouchToRay touch;
  public TerrainEngine engine;

  public Vector3 paintPosition;
  private Vector3 oPP;
  public Vector3 paintDirection;
  public float paintSize;
  public float paintOpacity;
  public float normalOrHeight;

	// Use this for initialization
	public override void Create(){

    Cycles.Insert(0,body);
    Cycles.Insert(1,life);
  }

  public override void Bind(){

    life.BindPrimaryForm("_VectorBuffer", vectors);
    life.BindAttribute("_PaintPosition", "paintPosition" , this);
    life.BindAttribute("_PaintDirection", "paintDirection" , this);
    life.BindAttribute("_PaintSize", "paintSize" , this);
    life.BindAttribute("_PaintOpacity", "paintOpacity" , this);
    life.BindAttribute("_NormalOrHeight", "normalOrHeight" , this);

    engine.BindData(life);
  }

  public override void WhileLiving( float v){

    if( active == true ){
    if( touch.Down == 1 ){
      oPP = paintPosition;
      paintPosition = trace.marker.position;
      if( touch.JustDown == 0 ){
      paintDirection = paintPosition - oPP;
      }else{
        paintDirection = Vector3.zero;
      }
      life.active = true;
    }else{
      if( touch.JustUp == 1 ){ 
      
        Save(); 
      }
      life.active = false;
      paintDirection = Vector3.zero;
    }
  }


  }

  public void Save(){

    float[] values = vectors.GetData();

    Color[] colors =  new Color[vectors.count];
    for( int i = 0; i < vectors.count; i ++ ){
      
      // extracting height
      float h = values[ i * vectors.structSize + 1 ] / engine.height;
      
      // extracting normals
      float x = values[ i * vectors.structSize + 6 ] * .5f + .5f;
      float z = values[ i * vectors.structSize + 8 ] * .5f + .5f;


      colors[i] = new Color( h,x,z,1);



    }

    engine.heightMap.SetPixels(colors,0);
    engine.heightMap.Apply(true);

    SaveTextureAsPNG( engine.heightMap , "Assets/HeightMap.png");

  }


public void TogglePaint(){
  normalOrHeight = (normalOrHeight+1)%3;
}

public void SetBrushSize(Slider s){
  paintSize = s.value * 100;
}
public void SetBrushOpacity(Slider s){
  paintOpacity = s.value;
}

  public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
   {
       byte[] _bytes =_texture.EncodeToPNG();
       System.IO.File.WriteAllBytes(_fullPath, _bytes);
       Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
   }



}
