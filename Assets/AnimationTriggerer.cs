using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerer : MonoBehaviour {

  public Animator anim;
  
  public BookObject bookObject;
  public WandParticles particles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void FallToSleep(){
    anim.SetTrigger("FallToSleep");
  }

  public void PickUp(){

    bookObject.Hide();

    particles.Emit( bookObject.transform.position );
    print("WHOA");
  
  }

  public void UseBook(){
  
  
    particles.Emit( bookObject.transform.position );
    //anim.speed = 0.01f;
    anim.SetTrigger("PutAway");

  }

  public void BackToNormal(){
    anim.speed = 1;
  }


}
