using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookLock : LifeForm {

  public BookObject book;
  public Transform bookRenderer;

  public Transform camera;
  public Ursula ursula;
  public RotateXY controls;

  public float lockSpeed;
  public bool locking;
  public bool locked;
  public float lockingDist;

  private float lockStartTime;

  public Vector3 currentTargetPosition;
  private Vector3 tmpPos;
  private Quaternion tmpRot;

  public Vector3 bottomLeft;
  public Vector3 bottomRight;
  public Vector3 topLeft;
  public Vector3 topRight;

  public Transform bottomLeftRepresent;
  public Transform bottomRightRepresent;
  public Transform topLeftRepresent;
  public Transform topRightRepresent;

  public override void OnGestate(){
    SetSize();
  }

  public void SetSize(){

    float _ratio = (float)Screen.width / (float)Screen.height;
    float border = .02f;

    Camera cam = Camera.main;

    Vector3  tmpP = cam.transform.position;
    Quaternion tmpR = cam.transform.rotation;

    cam.transform.position = this.transform.position;//transform;
    cam.transform.rotation = this.transform.rotation;//transform;

    bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3( border ,_ratio *border,lockingDist));  
    bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1- border,_ratio *border,lockingDist));
    topLeft = Camera.main.ViewportToWorldPoint(new Vector3(border,1-_ratio * border,lockingDist));
    topRight = Camera.main.ViewportToWorldPoint(new Vector3(1-border,1-_ratio * border,lockingDist));

    bottomLeft = transform.InverseTransformPoint(bottomLeft);
    bottomRight = transform.InverseTransformPoint(bottomRight);
    topLeft = transform.InverseTransformPoint(topLeft);
    topRight = transform.InverseTransformPoint(topRight);

    bottomLeftRepresent.localPosition   = bottomLeft;
    bottomRightRepresent.localPosition  = bottomRight;
    topLeftRepresent.localPosition      = topLeft;
    topRightRepresent.localPosition     = topRight;
    
    //bookRenderer.localScale = new Vector3( (bottomLeft - bottomRight ).x , (topLeft - bottomLeft).y
    //normal = transform.forward;


    //up = -(bottomLeft - topLeft).normalized;
    //right = -(bottomLeft - bottomRight).normalized;


    bookRenderer.transform.localScale = new Vector3( (bottomLeft - bottomRight).magnitude , (bottomLeft - topLeft).magnitude , .02f );

//    width = (bottomLeft - bottomRight).magnitude;
 //   height = (bottomLeft - topLeft).magnitude;
    

    cam.transform.position = tmpP;
    cam.transform.rotation = tmpR;

  }


  public void OnSwipe( float v ){

      if( locked ){
        UnlockBook();
      }
    
  }






public void LockBook(){
  //locked= true;
  locking = true;
  lockStartTime = Time.time;

  tmpPos = camera.position;
  tmpRot = camera.rotation;
    controls.enabled = false;

}
public void UnlockBook(){
  locked = false;
  controls.enabled = true;
  controls.SetAfterPage();
}

public void OnLock(){
  print("LOCKEDDd");
  locked = true;
  locking = false;
}

public override void _WhileLiving(float x){

if( locking ){

  float val = ( Time.time - lockStartTime ) / lockSpeed;
  BirthLerp(val);

  if( val >= 1 ){ OnLock(); }
}

if( locked == true ){
  BirthLerp(1);
}

}



/*

  Helpers

*/
  
public void BirthLerp(float v ){
  v = v * v * (3 - 2 * v);
  camera.position = Vector3.Lerp( tmpPos , transform.position - transform.forward * lockingDist, v );
  camera.rotation = Quaternion.Lerp( tmpRot , transform.rotation , v );
}


}
