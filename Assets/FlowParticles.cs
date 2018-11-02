using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowParticles : Particles {

  public PaintVectors vectors;


  public override void SetCount () {
	 count = vectors.count;	
	}

  public override void Embody(){

    int index = 0;

    float[] values = new float[count*structSize];
    for( int i = 0; i < count; i ++ ){

      int x = i % vectors.width;
      int y = i / vectors.width;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = (float)x/(float)vectors.width;
      values[ index ++ ] = (float)y/(float)vectors.width;


      values[ index ++ ] = Random.Range(0,.999f);
      values[ index ++ ] = (float)i/(float)count;
    }

    SetData( values );

  }


}
