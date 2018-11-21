using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : LifeForm {

  public List<Page> pages;

  public Animator anim;
  public float animationState;
  public BookTextParticles text;
  public AudioPlayer audio;

  public AudioClip frameHoverOverClip;
  public AudioClip frameHoverOutClip;
  public AudioClip pageLockedClip;
  public AudioClip pageStartClip;
  public AudioClip pageEndClip;



  public int lastPage;
  public int currentPage;
  public int nextPage;
  

  public int startPage = -1;
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
  public float fade;
  public float pageAlive;

  [HideInInspector]public Vector3 up;
  [HideInInspector]public Vector3 right;
  [HideInInspector]public Vector3 forward;
  [HideInInspector]public Vector3 ursulaPos;

  public bool ursulaHovered = false;
  public bool frameHovered = false;

  public Material frameMaterial;


  // STIFLE IS TRUE!


	// Use this for initialization
	public override void Create () {

    currentPage = startPage;
    
    int id = 0;
    foreach( Page page in pages){
      page._Create();
      page.frame.borderLine.material = frameMaterial;
      id++;
    }

    //pages[currentPage]._OnGestate();
//
		//gestateTime = Time.time;

    
	}
	

  public override void OnBirthed(){

    pages[currentPage]._OnGestate();
        gestateTime = Time.time;
    StartPage();
  }


	// Update is called once per frame
	void Update () {
		
    anim.SetFloat("Test", animationState );
	}


  public void RayMove(Ray ray){

      LayerMask mask = LayerMask.GetMask("Ursula");

      RaycastHit hit;
      if( Physics.Raycast( ray , out hit, 100 ,mask)){
        if( ursulaHovered == false ){ UrsulaHoverOver(); }
      }else{
        if( ursulaHovered == true ){ UrsulaHoverOut(); }
      }

      if( pages[currentPage].collider.Raycast( ray , out hit, 100)){
        if( frameHovered == false ){ FrameHoverOver(); }
      }else{
        if( frameHovered == true ){ FrameHoverOut(); }
      }

  }


  public void FrameHoverOver(){
    frameHovered = true;

     audio.Play(frameHoverOverClip);
    pages[currentPage].frame.borderLine.material.SetFloat("_Hovered" , 1 );
  }

  public void FrameHoverOut(){
    frameHovered = false;
     audio.Play(frameHoverOutClip);
    pages[currentPage].frame.borderLine.material.SetFloat("_Hovered" , 0 );
  }


  public void UrsulaHoverOver(){
    ursulaHovered = true;
    ursula.HoverOver();
  }

  public void UrsulaHoverOut(){
    ursulaHovered = false;
    ursula.HoverOut();
 }




  public void CheckRay( Ray ray ){

    RaycastHit hit;
   
    if( pages[currentPage].living ){
      if(ursulaHovered) EndPage();
    }else{
      if(frameHovered) StartPage();
      if(ursulaHovered) controls.ToggleCloseFar();
    }

  }

 
public override void _WhileDebug(){
  foreach( Page p in pages){
    p._WhileDebug();
  }
  DoDebug();
}



void SetPage(){

  if( currentPage == 0 ){
    text.Set(pages[currentPage].text);
    text.PageStart();
  }else{
    text.PageStart();
  }

}
public void StartPage(){


  Page p = pages[currentPage];
  p._OnBirth();
  birthTime = Time.time;
  tmpPos = camera.position;
  tmpRot = camera.rotation;

  tmpPos2 = ursula.transform.position;
  tmpRot2 = ursula.transform.rotation;

  tmpAnimationState = animationState;

  controls.enabled = false;

   audio.Play(pageStartClip);

   ursula.Lock( p , p.birthSpeed );
 
}

public void LivePage(Page cp){

        cp._OnBirthed();
        cp._OnLive();
        SetPage();
        audio.Play(pageLockedClip);
}

public void EndPage(){
  
  pages[currentPage]._OnLived();
  pages[currentPage]._OnDie();

  controls.enabled = true;
  controls.SetAfterPage();

  lastPage = currentPage;
  deathTime = Time.time;
  pageActive = 0;

  ursula.Unlock();
  
  if( currentPage < (pages.Count-1) ){
    currentPage += 1;
    nextPage = currentPage + 1;
    gestateTime = Time.time;
    pages[currentPage]._OnGestate();
  }else{
    print("END");
  }

  audio.Play(pageEndClip);


}

public void DiePage( Page lp , Page cp ){

  lp._OnDied();
  text.Set(cp.text);
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
      if( v >= 1 ){
        LivePage(cp);
      }

    }

    if( cp.living ){
      float v = 1;
      pageActive = 1;
      cp._WhileLiving(v);
      pageAlive += .01f;
    }else{

      pageAlive -= .01f;
    }

    pageAlive = Mathf.Clamp( pageAlive , 0 , 1 );

    Page lp = pages[lastPage];
    
    if( lp.dying ){
    
      float v = (Time.time - deathTime ) / lp.deathSpeed;
      lp._WhileDying(v);
    
      if( v >= 1 ){
        DiePage(lp,cp);
      }

    }

    currentTargetPosition = cp.subjectTarget.position;

    anim.SetFloat("Test", animationState );

}



/*

  Helpers

*/
  
public void BirthLerp(Page page , float v ){

  v = v * v * (3 - 2 * v);
  camera.position = Vector3.Lerp( tmpPos , page.transform.position , v );
  camera.rotation = Quaternion.Lerp( tmpRot , page.transform.rotation , v );//Vector3.Lerp( tmpPos2 , page.subjectTarget.position , v );
  animationState = Mathf.Lerp( tmpAnimationState , page.animationState , v );
}






}
