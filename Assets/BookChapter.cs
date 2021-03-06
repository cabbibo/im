﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookChapter : Cycle {

  public Material frameMaterial;


  public BookStory story;

 // public bool BookChapter;

  public List<Page> pages;
  
  public float startDist;
  public float endGestateDist;
  private float distToStart = 10000;
  private float oDistToStart = 10000;

  public Transform start;

  public int lastPage;
  public int currentPage;
  public int nextPage;
  

  public int startPage = -1;

  public bool chapterStarted = false;
  public UnityEvent OnActivateEvent;
  public UnityEvent OnDeactivateEvent;
  



  // Use this for initialization
  public override void Create () {

    //print("hellllooo");
    
    currentPage = startPage;
    
    int id = 0;
    foreach( Page page in pages){
      page._Create();
      page.frame.borderLine.material = frameMaterial;
      page.subjectTarget.GetComponent<MeshRenderer>().enabled = false;
      
      if( id > 0 ){ page.previousPage = pages[id-1]; }
      id++;
      if( id < pages.Count ){
//        print("Has Next Page");
        page.nextPage = pages[id];
      }

    }

    
  }
  


  /*

  These two will be for turning on and off simulations / visuals / etc.

  will also be used for saying that other chapters can now be completed ( aka activate a chapter that before didn't exist )

  They may also have to be used in the future for created and destroying buffers if we run
  out of GPU memory....

  */
  public void ActivateChapter(){

    story.SetChapter( this );

    pages[currentPage]._OnGestate();
    pages[currentPage].collider.enabled = true;

    OnActivateEvent.Invoke();
  }

  public void DeactivateChapter(){

    chapterStarted = false;
    story.inChapter = false;
    story.currentChapter = null;
    OnDeactivateEvent.Invoke();
  } 


  /*

    Need to see if we are close enough to activate or deactivate the page 

  */
  public override void WhileLiving( float v ){




  }



   // propogate debug
public override void _WhileDebug(){
  foreach( Page p in pages){
    p._WhileDebug();
  }
  DoDebug();
}



}
