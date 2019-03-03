using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public Story story;

    public bool hasFallen;
    public bool hasPickedUpBook;
    public bool hasSeenFirstMonolith;
    public bool hasSeenSpacePuppy;

    public void SetTrue( string s ){
      this.GetType().GetField(s).SetValue(this, true);
    }

    public void SetFalse( string s ){
      this.GetType().GetField(s).SetValue(this, false);
    }


    public void SetStartValues( int val ){

      print("vAL:LEE : " + val );

      if( val > 0 ){ hasFallen = true; }
      if( val > 1 ){ hasPickedUpBook = true; story.book.TransferParent(); }
      if( val > 2 ){ hasSeenFirstMonolith = true; }
      if( val > 3 ){ hasSeenSpacePuppy = true; }


    }


}
