using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookStory : LifeForm {

  public List<BookChapter> chapters;




  public AudioPlayer audio;

 // public BookTextParticles text;
  public AudioClip frameHoverOverClip;
  public AudioClip frameHoverOutClip;
  public AudioClip pageLockedClip;
  public AudioClip pageStartClip;
  public AudioClip pageEndClip;


  public bool skipChapter;
  public Page currentPage;
  public Page lastPage;

  public int startChapter;

  public BookChapter currentChapter;
  public BookChapter lastChapter;



  private float gestateTime;
  private float birthTime;
  private float deathTime;


  public float pageActive;
  public float fade;
  public float pageAlive;


  public bool ursulaHovered = false;
  public bool frameHovered = false;
  public bool bookHovered = false;
  public bool inChapter = false;



  // STIFLE IS TRUE!


  // Use this for initialization
  public override void Create () {
    foreach( BookChapter c in chapters){
      Cycles.Add(c);
    }
  }
  

  public void SetChapter( BookChapter chapter ){
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
    //StartPage();
  }




  public void RayMove(Ray ray){
      
      RaycastHit hit;
      
      if( currentPage.collider.Raycast( ray , out hit, 100)){
        if( frameHovered == false ){ FrameHoverOver(); }
      }else{
        if( frameHovered == true ){ FrameHoverOut(); }
      }


     /* if( book.collider.Raycast( ray , out hit, 100)){
        if( bookHovered == false ){ BookHoverOver(); }
      }else{
        if( bookHovered == true ){ BookHoverOut(); }
      }*/



  }

  public void BookHoverOver(){
    bookHovered = true;
  }

  public void BookHoverOut(){
    bookHovered = false;
  }


  public void FrameHoverOver(){
    frameHovered = true;

    //audio.Play(frameHoverOverClip);
    currentPage.frame.borderLine.material.SetFloat("_Hovered" , 1 );
  }

  public void FrameHoverOut(){
    frameHovered = false;
    audio.Play(frameHoverOutClip);
    currentPage.frame.borderLine.material.SetFloat("_Hovered" , 0 );
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



public void StartPage(){

  Page p = currentPage;

  // Make sure that its been gestated
  // TODO PROBLEMS
  if( p.gestated == false ){
    p._OnGestated();
  }

  if( currentChapter.chapterStarted == false ){ currentChapter.chapterStarted = true; }

  
  
  p._OnBirth();
  birthTime = Time.time;

  audio.Play(pageStartClip);

 
}

public void LivePage(Page cp){

  cp._OnBirthed();
  cp._OnLive();
      
  //text.Set(currentPage.text);
  //text.PageStart();

  audio.Play(pageLockedClip);

}



public void PreviousPage(){
  
  currentPage._OnLived();


  deathTime = Time.time;
  pageActive = 0;


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
    BookChapter tmp = currentChapter;
    currentChapter.DeactivateChapter();
    tmp.ActivateChapter();
  

  }


  audio.Play(pageEndClip);


}


public void NextPage(){
  
  currentPage._OnLived();
  currentPage._OnDie();


  deathTime = Time.time;
  pageActive = 0;

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
  

  }


  audio.Play(pageEndClip);


}

public void DiePage( Page lp , Page cp ){
  lp._OnDied();
  //text.Set(cp.text);
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
   
    DoLiving(x);

}

}
