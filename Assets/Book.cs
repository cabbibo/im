using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : LifeForm {

  public List<Page> pages;

  public Animator anim;
  public float animationState;
  public BookTextParticles text;

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

  public float pageActive;

  [HideInInspector]public Vector3 up;
  [HideInInspector]public Vector3 right;
  [HideInInspector]public Vector3 forward;
  [HideInInspector]public Vector3 ursulaPos;


  // STIFLE IS TRUE!


	// Use this for initialization
	public override void Create () {

    if( skipTo >=  0 ){
      currentPage = skipTo;
    }
    
    foreach( Page page in pages){
     // Cycles.Add(page);
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

 
public override void _WhileDebug(){
  foreach( Page p in pages){
    p._WhileDebug();
  }
  DoDebug();
}
public void StartPage(){

  pages[currentPage]._OnBirth();
  pageActive = 1;
  text.Set(pages[currentPage].text);
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
  pageActive = 0;
  
  if( currentPage < (pages.Count-1) ){
    currentPage += 1;
    nextPage = currentPage + 1;
    gestateTime = Time.time;
    pages[currentPage]._OnGestate();
  }else{
    print("END");
  }


}


public override void _WhileLiving(float x){



    right = camera.right;
    up = camera.up;
    forward = camera.forward;
    ursulaPos = ursula.soul.position;

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
   
      Vector3 dif =  (cp.subjectTarget.position - ursula.transform.position) * 2.2f; 
      if( dif.magnitude > 1 ){ ursula.velocity += dif; }

      if( v >= 1 ){
        cp._OnBirthed();
        cp._OnLive();
      }

    }

    if( cp.living ){
      float v = 1;


      Vector3 dif =  (cp.subjectTarget.position - ursula.transform.position) * 2.2f; 
      if( dif.magnitude > 1 ){ ursula.velocity += dif; }
      
      cp._WhileLiving(v);
    }

    Page lp = pages[lastPage];
    if( lp.dying ){
      float v = (Time.time - deathTime ) / lp.deathSpeed;
      lp._WhileDying(v);
      
      if( v >= 1 ){
        lp._OnDied();
        //lp._Destroy();
       // text.Set(cp.text);
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
