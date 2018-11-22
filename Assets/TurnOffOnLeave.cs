using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnLeave : Cycle{


  public float fadeDistanceStart;
  public float fadeDistanceEnd;
  public Ursula ursula;
  public Transform center;
  public FadeIn fade;

  public bool beReady;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public override void WhileLiving( float v ) {
	
    //print("hi");
    if( beReady == true ){

     // print("reallyHi");

    float dif = (ursula.position - center.position).magnitude;
    float fadeValue = ( dif - fadeDistanceStart) / ( fadeDistanceEnd-fadeDistanceStart);

    fade.alpha = fadeValue;
    if( fadeValue > 1 ){
      TriggerEnd();
    }
  }

         if (Input.GetKey("escape"))
        {
          Application.Quit();
        }
	}


  public void TriggerEnd(){
//    print("ENDED");


    Application.Quit();
  }

  public void ReadyMe(){
    beReady = true;
  }


}
