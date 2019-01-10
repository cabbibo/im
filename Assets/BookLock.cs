using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookLock : LifeForm {

  public BookObject book;

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
