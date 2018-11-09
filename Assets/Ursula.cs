using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ursula : MonoBehaviour {

  public Collider bubble;
  public MeshRenderer bubbleRenderer;
  public bool bubbleActive;

  public Book book;

  public Color activeColor;
  public Color passiveColor;

	// Use this for initialization
	void Start () {
		DeactivateBubble();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseOver(){
    bubbleRenderer.material.SetColor("_OutlineColor" , activeColor );
    bubbleRenderer.material.SetFloat("_Threshold" , .01f );
    bubbleRenderer.material.SetFloat("_Fade" , .2f );
  }
  
  void OnMouseExit(){
    bubbleRenderer.material.SetColor("_OutlineColor" , passiveColor );
    bubbleRenderer.material.SetFloat("_Threshold" , .1f );
    bubbleRenderer.material.SetFloat("_Fade" , 3 );
  }

  void OnMouseDown(){

    if( bubbleActive ){
      book.Release();
    }
  }

  public void ActivateBubble(){
    bubbleRenderer.enabled = true;
    bubbleActive = true;
  }


    public void DeactivateBubble(){
    bubbleRenderer.enabled = false;
    bubbleActive = false;
  }
}
