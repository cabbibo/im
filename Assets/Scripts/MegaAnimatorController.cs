using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaAnimatorController : MonoBehaviour {

  public Animator A;
  
  public float test;


	// Use this for initialization
	void Start () {
	 A = GetComponent<Animator>();	

//    A.Play("Sleeping");
	}
	
	// Update is called once per frame
	void Update () {
		

    AnimatorStateInfo info = A.GetCurrentAnimatorStateInfo(0);
    //A.SetFloat("Test",test);


    if( transform.position.y < 10 && test < 1 ){
    	test = 1.1f;
    }


	}



}
