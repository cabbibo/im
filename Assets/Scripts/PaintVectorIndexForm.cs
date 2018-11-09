using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintVectorIndexForm : IndexForm {

  public int width;
  public override void SetCount(){ 
    width = ((PaintVectors)toIndex).width;
    count = (width-1)*(width-1) * 3 * 2; 
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    for( int i = 0; i < width-1; i++ ){
    for( int j = 0; j < width-1; j++ ){
        values[ index ++ ] = i*width + j;
        values[ index ++ ] = i*width + j+1;
        values[ index ++ ] = ((i+1)*width) + j+1;
        values[ index ++ ] = i*width + j;
        values[ index ++ ] = ((i+1)*width) + j+1;
        values[ index ++ ] = ((i+1)*width) + j;
    }
    }
    SetData(values);
  }


}
