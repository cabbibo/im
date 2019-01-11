using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;





public class CollisionChecker : MonoBehaviour {

  public UnityEvent ursulaHoverOverEvent;
  public UnityEvent ursulaHoverOutEvent;
  public Vector3Event ursulaClickedEvent;

  public UnityEvent bookHoverOverEvent;
  public UnityEvent bookHoverOutEvent;
  public Vector3Event bookClickedEvent;

  public UnityEvent monolithHoverOverEvent;
  public UnityEvent monolithHoverOutEvent;
  public Vector3Event monolithClickedEvent;

  public UnityEvent frameHoverOverEvent;
  public UnityEvent frameHoverOutEvent;
  public Vector3Event frameClickedEvent;
  
  public UnityEvent terrainHoverOverEvent;
  public UnityEvent terrainHoverOutEvent;
  public Vector3Event terrainClickedEvent;

  public bool terrainHovered;
  public bool oTerrainHovered;

  public bool ursulaHovered;
  public bool oUrsulaHovered;

  public bool bookHovered;
  public bool oBookHovered;

  public bool monolithHovered;
  public bool oMonolithHovered;

  public bool frameHovered;
  public bool oFrameHovered;

  public Transform Wand;
  public TerrainEngine terrain;

  private Vector3 ro;
  private Vector3 rd;

	// Use this for initialization
	void Start () {
		
	}

  void OnMouseDown(){
    if( terrainHovered ){

    }
  }
	
	// Update is called once per frame
	void Update () {
    //print( t);

    Vector2 p =  Input.mousePosition;///Input.GetTouch(0).position;
    ro = Camera.main.ScreenToWorldPoint( new Vector3( p.x , p.y , Camera.main.nearClipPlane ) );
    rd = -(Camera.main.transform.position - ro).normalized;
    

    oUrsulaHovered = ursulaHovered;
    oBookHovered = bookHovered;
    oMonolithHovered = monolithHovered;
    oTerrainHovered = terrainHovered;
    oFrameHovered = frameHovered;

    ursulaHovered = false;
    bookHovered = false;
    monolithHovered = false;
    terrainHovered = false;
    frameHovered = false;

    RaycastHit hit;
    if( Physics.Raycast(ro,rd, out hit, Mathf.Infinity)){

      string tag = hit.collider.gameObject.tag;

      if( tag == "Ursula"){ ursulaHovered = true;  
      }else if( tag == "Book"){ bookHovered = true; 
      }else if( tag == "Monolith"){ monolithHovered = true; 
      }else if( tag == "Frame"){ frameHovered = true; 
      }else{ print("WHAT IN THE HECK : " + tag ); }
    
      Wand.position = hit.point;
    
    }else{
    
      bool terrainHit= false; 
      Vector3 hitPos = terrain.Trace(ro,rd,out terrainHit); 
//      print( terrainHit );
    
      if( terrainHit ){
        terrainHovered = true;
      }else{

      }
      
      Wand.position = hitPos;

    }


    if( oUrsulaHovered == false && ursulaHovered == true ){
      ursulaHoverOverEvent.Invoke();
    }

    if( oUrsulaHovered == true && ursulaHovered == false ){
      ursulaHoverOutEvent.Invoke();
    }

    bool t = Input.GetMouseButtonDown(0);

    if( t == true && terrainHovered == true ){ terrainClickedEvent.Invoke( Wand.position ); }
    if( t == true && ursulaHovered == true ){ ursulaClickedEvent.Invoke( Wand.position ); }
    if( t == true && monolithHovered == true ){ monolithClickedEvent.Invoke( Wand.position ); }
    if( t == true && bookHovered == true ){ bookClickedEvent.Invoke( Wand.position ); }
    if( t == true && frameHovered == true ){ frameClickedEvent.Invoke( Wand.position ); }


  }
}
