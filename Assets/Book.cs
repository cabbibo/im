using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour {

  public List<Page> pages;

  public Animator anim;
  public float animationState;

  public int currentPage;
  public int nextPage;
  public Ursula ursula;

  public RotateXY controls;



	// Use this for initialization
	void Start () {

    pages[currentPage].Prepare();
		
	}
	
	// Update is called once per frame
	void Update () {
		
    anim.SetFloat("Test", animationState );
	}

  public void CheckRay( Ray ray ){

    RaycastHit hit;

   
    if( pages[currentPage].active ){

    


    }else{
      if( pages[currentPage].collider.Raycast( ray , out hit, 100)){
       Steal();
      }else{
        print("NAH");
      }


    }

  }

 

 public void Steal(){
  pages[currentPage].Activate();
  controls.enabled = false;
  ursula.ActivateBubble();
 }

public void Release(){
  pages[currentPage].Deactivate();
  ursula.DeactivateBubble();
  controls.enabled = true;
  currentPage += 1;
  pages[currentPage].Prepare();
  currentPage %= pages.Count;
  //CheckTransitions();
}


public void CheckTransitions(){



}

}
