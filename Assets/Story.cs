using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : LifeForm {

  public List<Chapter> chapters;

  public StateMachine stateMachine;
  public Animator anim;
  public float animationState;
  public BookTextParticles text;
  public AudioPlayer audio;
  public BookObject book;

  public bool quickTransition;
  public bool skipChapter;

  public AudioClip frameHoverOverClip;
  public AudioClip frameHoverOutClip;
  public AudioClip pageLockedClip;
  public AudioClip pageStartClip;
  public AudioClip pageEndClip;



  public Page currentPage;
  public Page lastPage;

  public int startChapter;

  public Chapter currentChapter;
  public Chapter lastChapter;


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
  public bool bookHovered = false;
  public bool inChapter = false;



  // STIFLE IS TRUE!


	// Use this for initialization
	public override void Create () {
    foreach( Chapter c in chapters){
      Cycles.Add(c);
    }
	}
	

  public void SetChapter( Chapter chapter ){
    inChapter= true;
    lastChapter = currentChapter;
    currentChapter = chapter;
    SetPage( chapter.pages[chapter.currentPage] );
  }
  public void SetPage(Page page){
    lastPage = currentPage;
    currentPage = page;
    page._OnGestate();
  }

  public override void OnBirthed(){

    chapters[startChapter].ActivateChapter();
    stateMachine.SetStartValues( startChapter );
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


      if( currentPage.collider.Raycast( ray , out hit, 100)){
        if( frameHovered == false ){ FrameHoverOver(); }
      }else{
        if( frameHovered == true ){ FrameHoverOut(); }
      }


      if( book.collider.Raycast( ray , out hit, 100)){
        if( bookHovered == false ){ BookHoverOver(); }
      }else{
        if( bookHovered == true ){ BookHoverOut(); }
      }



  }

  public void BookHoverOver(){
    bookHovered = true;
  }

  public void BookHoverOut(){
    bookHovered = false;
  }


  public void FrameHoverOver(){
    frameHovered = true;

    audio.Play(frameHoverOverClip);
    currentPage.frame.borderLine.material.SetFloat("_Hovered" , 1 );
  }

  public void FrameHoverOut(){
    frameHovered = false;
    audio.Play(frameHoverOutClip);
    currentPage.frame.borderLine.material.SetFloat("_Hovered" , 0 );
  }


  public void UrsulaHoverOver(){
    ursulaHovered = true;
    ursula.HoverOver();
  }

  public void UrsulaHoverOut(){
    ursulaHovered = false;
    ursula.HoverOut();
 }



  public void OnSwipe( float v ){

    print( v );
    if( v < 0 ){
      if( !currentPage.birthing ){
        if( currentPage.living && !currentPage.restricted ){
          NextPage();
        }else{
          //StartPage();
        }
      }
    }else{
       if( !currentPage.birthing ){
        if( currentPage.living ){
          PreviousPage();
        }else{
          //StartPage();
        }
      }
    }
  }


  public void CheckStart(){
    if( !currentPage.living ){ StartPage(); }
  }


  public void CheckRay( Ray ray ){

    RaycastHit hit;
   


     //if(ursulaHovered) ursula.Emit();///.ToggleCloseFar();
     //if(ursulaHovered) ursula.LookAtBook();//controls.ToggleCloseFar();
     //if(frameHovered && !currentPage.living){ StartPage(); }
/*
    if( currentPage.living ){
      if(ursulaHovered) EndPage();
      if(bookHovered) EndPage();
    }else{
      if(frameHovered) StartPage();
      if(ursulaHovered) controls.ToggleCloseFar();
    }*/


  }

 
 // propogate debug
/*public override void _WhileDebug(){
  foreach( Chapter c in chapters){
    c._WhileDebug();
  }
  DoDebug();
}*/

public void StartPage(){

  Page p = currentPage;

  // Make sure that its been gestated
  // TODO PROBLEMS
  if( p.gestated == false ){
    p._OnGestated();
  }

  if( currentChapter.chapterStarted == false ){ currentChapter.chapterStarted = true; }

// TODO this is hacky af
  if( p.grounded ){
    ursula.SetGrounded();
    animationState = 5;
  }
  
  if( quickTransition ){
    p.birthSpeed = p.birthSpeed / 20;
    p.lockSpeed = p.lockSpeed * 20;
  }


  ursula.lockForce = p.lockSpeed;
  ursula.forceCutoffRadiusStart = p.lockStartRadius;
  ursula.forceCutoffRadiusEnd = p.lockEndRadius;

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
      
  text.Set(currentPage.text);
  text.PageStart();

  audio.Play(pageLockedClip);

}



public void PreviousPage(){
  
  currentPage._OnLived();


  deathTime = Time.time;
  pageActive = 0;

  ursula.Unlock();
  

  if ( currentPage.previousPage != null ){


    currentPage._OnDie();

    inChapter = true;

    print( "hasPreviousPAge");

    SetPage( currentPage.previousPage );
    
    currentPage._OnGestate();
    StartPage();


  }else{

    print( "No More Pages" );
    inChapter = false;
    controls.enabled = true;
    controls.SetAfterPage();
    Chapter tmp = currentChapter;
    currentChapter.DeactivateChapter();
    tmp.ActivateChapter();

    ursula.Unlock();
  

  }


  audio.Play(pageEndClip);


}


public void NextPage(){
  
  currentPage._OnLived();
  currentPage._OnDie();


  deathTime = Time.time;
  pageActive = 0;

  ursula.Unlock();
  

  if ( currentPage.nextPage != null ){

    inChapter = true;

    print( "hasNExtPAge");
    if( skipChapter ){

      SetPage( currentChapter.pages[currentChapter.pages.Count-1] );
    }else{

      SetPage( currentPage.nextPage );
    }

    currentPage._OnGestate();
    StartPage();

  }else{

    print( "No More Pages" );
    inChapter = false;
    controls.enabled = true;
    controls.SetAfterPage();

    ursula.Unlock();
  

  }


  audio.Play(pageEndClip);


}

public void DiePage( Page lp , Page cp ){
  lp._OnDied();
  text.Set(cp.text);
}

public void currentPageLiving(){

    Page cp = currentPage;  

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

}


public override void _WhileLiving(float x){

    right = camera.right;
    up = camera.up;
    forward = camera.forward;
    ursulaPos = ursula.soul.position;
    Page cp = currentPage;  
    currentPageLiving();

    pageAlive = Mathf.Clamp( pageAlive , 0 , 1 );

    Page lp = lastPage;
    
    if( lp != null && lp.dying ){
    
      float v = (Time.time - deathTime ) / lp.deathSpeed;
      lp._WhileDying(v);
    
      if( v >= 1 ){
        DiePage(lp,cp);
      }

    }

    currentTargetPosition = cp.subjectTarget.position;

    anim.SetFloat("Test", animationState );

    DoLiving(x);

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
