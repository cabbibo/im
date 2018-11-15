using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;



[System.Serializable]
public class Vector2Event : UnityEvent<Vector2>{}

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3>{}

[System.Serializable]
public class FloatEvent : UnityEvent<float>{}



[System.Serializable]
public class RayEvent : UnityEvent<Ray>{}

public class TouchToRay : MonoBehaviour {

  public bool sceneView;

  public Vector2Event OnSwipe;
  public FloatEvent OnSwipeHorizontal;
  public UnityEvent OnSwipeLeft;
  public UnityEvent OnSwipeRight;
  public FloatEvent OnSwipeVertical;
  public UnityEvent OnSwipeUp;
  public UnityEvent OnSwipeDown;
  public UnityEvent OnTap;
  public RayEvent   RayTap;
  public RayEvent   RayWhileDown;
  public RayEvent   RayMove;
  

  public Vector3 RayOrigin;
  public Vector3 RayDirection;
  public Ray ray;
  public float Down;
  public float oDown;
  public float JustDown;
  public float JustUp;
  public Vector2 startPos;
  public Vector2 endPos;

  public float startTime;
  public float endTime;
  // Use this for initialization

  public Vector2 p; 
  public Vector2 oP; 
  public Vector2 vel;

  public int touchID = 0;

  void Start(){}
  
   // Update is called once per frame
  void FixedUpdate () {

    oP = p;
    oDown = Down;

        p  =  GetPos();
      if (GetDown()){
        Down = 1;
        p  =  GetPos();
      }else{
        Down = 0;
        oP = p;
      }

      if( Down == 1 && oDown == 0 ){
          JustDown = 1;
          touchID ++;
          startTime = Time.time;
          startPos = p;
      }

      if( Down == 1 && oDown == 1 ){
        JustDown = 0;
      }


      if( Down == 0 && oDown == 1 ){
        JustUp = 1;
        endTime = Time.time;
        endPos = p;
        OnUp();
      }      

      if( Down == 0 && oDown == 0 ){
        JustUp = 0;
      }



      if( JustDown == 1 ){ oP = p; }
      vel = p - oP;



    if( sceneView == true ){
      if( Event.current != null){
      print(Event.current.mousePosition);
      Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
      RayOrigin = r.origin;
      RayDirection = r.direction;
    }
    }else{
      RayOrigin = Camera.main.ScreenToWorldPoint( new Vector3( p.x , p.y , Camera.main.nearClipPlane ) );
      RayDirection = -(Camera.main.transform.position - RayOrigin).normalized;
      ray.origin = RayOrigin;
      ray.direction = RayDirection;
      RayMove.Invoke( ray );

    }






  }



  Vector2 GetPos(){

    Vector2 p;
    
    #if UNITY_EDITOR  
      if( sceneView == true ){
        if( Event.current != null){
        p = Event.current.mousePosition;
        }else{
          p = Input.mousePosition;
        }
      }else{
        p  =  Input.mousePosition;///Input.GetTouch(0).position;
      }
    #else
        p  =  Input.GetTouch(0).position;
    #endif 

    return p;

  }

  bool GetDown(){
    bool p;  
    #if UNITY_EDITOR  


      if( sceneView == true ){
        p = Input.GetMouseButton (0);
      }else{
        p  =  Input.GetMouseButton (0);///Input.GetTouch(0).position;
      }
    #else
        p  =  (Input.touchCount > 0);
    #endif 

    return ( p  );
  
  }

  void OnUp(){
    float difT = endTime - startTime;
    Vector2 difP = endPos - startPos;


    float ratio = .01f * difP.magnitude / difT;

    if( ratio > 3 ){
      
      OnSwipe.Invoke( difP );  
     if( Mathf.Abs(difP.x) > Mathf.Abs(difP.y) ){
        OnSwipeHorizontal.Invoke(difP.x);
        if( difP.x < 0 ){
          OnSwipeLeft.Invoke();
        }else{
          OnSwipeRight.Invoke();
        }
     }else{
      OnSwipeVertical.Invoke(difP.y);
      if( difP.x < 0 ){
        OnSwipeUp.Invoke();
      }else{
        OnSwipeDown.Invoke();
      }
     } 
    }else{
      OnTap.Invoke();
      RayTap.Invoke(ray);
    }


   //print( difT );
   //print( difP );
   //print( ratio );
  }


   private bool IsPointerOverUIObject() {
     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
     List<RaycastResult> results = new List<RaycastResult>();
     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
     
     return results.Count > 0;
 }


}