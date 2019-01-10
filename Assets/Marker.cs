using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {


  public float canSelect;
  public float fadeSpeed;
  public float current;
  public Monolith monolith;
  public Monolith referenceMonolith;
  public Marker[] nextMarkers;

  public AudioClip hoverClip;
  public AudioClip selectClip;
  public AudioClip cantClip;

  public float hoverTime;
  public float touchTime;
  public float timeline;
  public bool selected;

  private Material mat;

  public LineRenderer lr;

  void OnMouseEnter(){
    print("hovver");
    AudioPlayer.instance.Play( hoverClip );
    mat.SetFloat("_Hovered",1);
    hoverTime = Time.time;
  }

  void OnMouseOver(){
    hoverTime = Time.time;
  }

  void OnMouseExit(){
    mat.SetFloat("_Hovered",0);
  }

  void OnMouseDown(){
    if( canSelect > .5f ){
      ToggleSelect();
    }else{
      CantSelect();
    }
  }

  void CantSelect(){
    AudioPlayer.instance.Play( cantClip );
  }

  void ToggleSelect(){
    selected = !selected;
    if( selected ){
      AudioPlayer.instance.Play(selectClip);
      monolith.OnSelect( this );
      for( int i = 0; i < nextMarkers.Length; i++ ){
        nextMarkers[i].lr.SetPosition( 0, this.transform.position );
        nextMarkers[i].lr.SetPosition( 1, nextMarkers[i].transform.position );
      }
    }else{
      monolith.OnDeselect( this );
    }

    float sVal = selected ? 1:0;

    mat.SetFloat("_Selected", sVal );
  
  }


	// Use this for initialization
	void Start () {
    mat = GetComponent<MeshRenderer>().material;		
    lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
    float v =  Time.time - hoverTime;	
	  v = 1 - Mathf.Clamp( v / fadeSpeed , 0 , 1 );
    mat.SetFloat("_HoverTime",v);
    mat.SetFloat("_Current",current);
    mat.SetFloat("_CanSelect", canSelect);
  }
}
