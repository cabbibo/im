using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : LifeForm {

  public List<Page> pages;

  public Animator anim;
  public float animationState;

  public int lastPage;
  public int currentPage;
  public int nextPage;
  

  public int skipTo = -1;
  public Transform camera;
  public Ursula ursula;
  public RotateXY controls;

  private float gestateTime;
  private float birthTime;
  private float deathTime;

  public Vector3 currentTargetPosition;
  private Vector3 tmpPos;
  private Quaternion tmpRot;

  private Vector3 tmpPos2;
  private Quaternion tmpRot2;

  private float tmpAnimationState;


  // STIFLE IS TRUE!


	// Use this for initialization
	public override void Create () {

        if( skipTo >=  0 ){
      currentPage = skipTo;
    }
    
    foreach( Page page in pages){
      page._Create();
    }

    pages[currentPage]._OnGestate();

		gestateTime = Time.time;

        if( skipTo >=  0 ){
      StartPage();
    }
    
	}
	


	// Update is called once per frame
	void Update () {
		
    anim.SetFloat("Test", animationState );
	}

  public void CheckRay( Ray ray ){

    RaycastHit hit;
   
    if( pages[currentPage].living ){

      if( ursula.bubble.Raycast( ray , out hit, 100)){
        EndPage();
      }else{
        print("NAH2");
      }


    }else{

      if( pages[currentPage].collider.Raycast( ray , out hit, 100)){
        StartPage();
      }else{
        print("NAH");
      }

    }

  }

 

public void StartPage(){

  pages[currentPage]._OnBirth();
  birthTime = Time.time;
  tmpPos = camera.position;
  tmpRot = camera.rotation;

  tmpPos2 = ursula.transform.position;
  tmpRot2 = ursula.transform.rotation;

  tmpAnimationState = animationState;

  controls.enabled = false;
  ursula.ActivateBubble();
 
}

public void EndPage(){
  
  pages[currentPage]._OnLived();
  pages[currentPage]._OnDie();

  ursula.DeactivateBubble();
  controls.enabled = true;

  lastPage = currentPage;
  deathTime = Time.time;
  
  if( currentPage < (pages.Count-1) ){
    currentPage += 1;
    nextPage = currentPage + 1;
    gestateTime = Time.time;

    pages[currentPage]._Create();
    pages[currentPage]._OnGestate();
  }else{
    print("END");
  }


}


public override void _WhileLiving(float x){

    Page cp = pages[currentPage];
    if( cp.gestating ){
      float v = (Time.time - gestateTime ) / cp.gestateSpeed;
      cp._WhileGestating(v);
      if( v >= 1 ){
        cp._OnGestated();
      }
    }


    if( cp.birthing ){
      float v = (Time.time - birthTime ) / cp.birthSpeed;
      cp._WhileBirthing(v);

      BirthLerp( cp , v );

      if( v >= 1 ){
        cp._OnBirthed();
        cp._OnLive();
      }

    }

    if( cp.living ){
      float v = 1;


      Vector3 dif =  (cp.subjectTarget.position - ursula.transform.position) * 2.2f; 

      //dif = new Vector3( dif.x,0 , dif.z);
      if( dif.magnitude > 1 ){ ursula.velocity += dif; }
      cp._WhileLiving(v);
    }

    Page lp = pages[lastPage];
    if( lp.dying ){
      float v = (Time.time - deathTime ) / lp.deathSpeed;
      lp._WhileDying(v);
      
      if( v >= 1 ){
        lp._OnDied();
        lp._Destroy();
      }

    }

    currentTargetPosition = cp.subjectTarget.position;



    anim.SetFloat("Test", animationState );

}



/*

  Helpers

*/
  
public void BirthLerp(Page page , float v ){
  camera.position = Vector3.Lerp( tmpPos , page.transform.position , v );
  camera.rotation = Quaternion.Lerp( tmpRot , page.transform.rotation , v );//Vector3.Lerp( tmpPos2 , page.subjectTarget.position , v );
  animationState = Mathf.Lerp( tmpAnimationState , page.animationState , v );
}






}
