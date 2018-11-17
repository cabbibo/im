using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexParticleTransferTris : IndexForm {

  public override void SetCount(){
    count = (toIndex.count/7) * 6 * 3;
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    // 1-0
    // |/|
    // 3-2
    for( int i = 0; i < count/18; i++ ){
        int bID = i * 7;
        for( int j = 0; j < 6; j++ ){
          values[ index ++ ] = bID ;
          values[ index ++ ] = bID + 1 + j;
          values[ index ++ ] = bID + 1 + ((j+1)%6);
        }
    }
    SetData(values);
  }

}

