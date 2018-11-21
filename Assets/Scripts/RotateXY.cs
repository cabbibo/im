using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RotateXY : MonoBehaviour {

  public float rotateSizeX;
  public float rotateSizeY;

  public TerrainEngine engine;

  public float angle;



  public float height;
  public float radius;


public bool far;
  public float closeRadius;
  public float closeHeight;

  public float farRadius;
  public float farHeight;

  public float closeFarChangeSpeed;

  public Transform subject;
  public Transform toRotate;
  public Transform cameraHolder;
  public Transform camera;

  public float rotSpeed;
  public float moveSpeed;
  public float lookAtSpeed;
  public float verticleDelta;

  public float stealBackTime;
private Quaternion targetRot;
private Vector3 target;

private float hDelta1;
private float hDelta;

private float pageEndTime;

  // Use this for initialization
  void Start () {
    
    hDelta1 = 0;
  }

  public void ToggleCloseFar(){

    if( far ){
      SetClose();
    }else{
      SetFar();
    }
  }

  void SetClose(){
    far = false;
  }
  

  void SetFar(){
    far = true;
  }

  // Update is called once per frame
  void Update () {
      // Read the user input
    var x = CrossPlatformInputManager.GetAxis("Mouse X");
    //var y = CrossPlatformInputManager.GetAxis("Mouse Y");

   // print( x );
   


    if( far ){
      radius = Mathf.Lerp(radius,farRadius , closeFarChangeSpeed * Time.deltaTime);
      height = Mathf.Lerp(height,farHeight , closeFarChangeSpeed * Time.deltaTime);
    }else{

      radius = Mathf.Lerp(radius,closeRadius , closeFarChangeSpeed  * Time.deltaTime);
      height = Mathf.Lerp(height,closeHeight , closeFarChangeSpeed  * Time.deltaTime);
    }





    // This gets out up or down motion
    hDelta1 = (Input.mousePosition.y / Screen.height) - .5f;
    hDelta = hDelta1; //Mathf.Lerp( hDelta , hDelta1 , Time.deltaTime * rotSpeed );
    target = subject.position  + Vector3.up * (height - hDelta * verticleDelta);



    //Moves our angle depending on  our mouse x poisition;
    angle = (Input.mousePosition.x / Screen.width) - .5f;//rotateSizeX * x;
    angle *= 90;

    toRotate.localPosition = Vector3.forward * radius;
    angle = Mathf.Clamp( angle , -90 , 90 );

    // Adds the rotation of the subject to the rotaion of our rig 
    targetRot = Quaternion.Euler(0, subject.eulerAngles.y + angle + 180 , 0);
    





    float steal = Mathf.Clamp( (Time.time - pageEndTime) / stealBackTime , 0 , 1);

    float tDelta = Time.deltaTime * steal * steal;
    float tODelta = Time.deltaTime * (1-steal * steal);

    // moves our target towards our up or down location
    transform.position = Vector3.Lerp( transform.position , target , moveSpeed * tDelta ) ;//subject.position  + Vector3.up * (height - hDelta * 3);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,  rotSpeed*tDelta);
    //cameraHolder.rotation = Quaternion.Slerp( cameraHolder.rotation, Quaternion.LookRotation( subject.position - cameraHolder.position ) , lookAtSpeed * tDelta );
    





    // Makes sure that we dont intersect w/ the terrain
    float h = engine.SampleHeight( cameraHolder.position );

    Vector3 tlocalPos= Vector3.zero;
    if( cameraHolder.position.y < h + 5 ){

      float d = (h+5) - cameraHolder.position.y;
      tlocalPos = Vector3.up * d; //target = new Vector3( target.x , h +3 , target.z );
    }

    cameraHolder.localPosition = Vector3.Lerp( cameraHolder.localPosition , tlocalPos  , lookAtSpeed * tDelta );
//    camera.localRotation = Quaternion.Slerp( camera.localRotation , Quaternion.identity , lookAtSpeed * tDelta );  
    camera.localPosition = Vector3.Lerp( camera.localPosition , Vector3.zero  , lookAtSpeed * tDelta );
  
camera.rotation = Quaternion.Slerp( camera.rotation, Quaternion.LookRotation( (subject.position - camera.position) ) , lookAtSpeed * Time.deltaTime );
   

   // angle *= .95f;
  
  }


  public void SetAfterPage(){

    Vector3 tmpP = camera.position;
    Quaternion tmpR = camera.rotation;
    
toRotate.localPosition = Vector3.forward * radius;
    target = subject.position  + Vector3.up * height; 
    transform.position = target;

    targetRot = Quaternion.Euler(0, subject.eulerAngles.y + angle + 180 , 0);
    transform.rotation = targetRot;


    float h = engine.SampleHeight( cameraHolder.position );

    Vector3 tlocalPos= Vector3.zero;
    if( cameraHolder.position.y < h + 5 ){

      float d = (h+5) - cameraHolder.position.y;
      tlocalPos = Vector3.up * d; //target = new Vector3( target.x , h +3 , target.z );
    }

    //cameraHolder.localPosition = tlocalPos;


    //cameraHolder.rotation =  Quaternion.LookRotation( subject.position - cameraHolder.position );
    //cameraHolder.rotation =  Quaternion.LookRotation( subject.position - cameraHolder.position );

    camera.position = tmpP;
    camera.rotation = tmpR;



    pageEndTime = Time.time;


  }
}
