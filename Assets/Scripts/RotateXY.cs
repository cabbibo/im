using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RotateXY : MonoBehaviour {

  public float height;
  public float rotateSizeX;
  public float rotateSizeY;

  public float angle;
  public float radius;
  public Transform subject;
  public Transform toRotate;
  public Transform cameraHolder;
  public Transform camera;

  public float rotSpeed;
  public float moveSpeed;
  public float lookAtSpeed;
  public float verticleDelta;
private Quaternion targetRot;
private Vector3 target;

private float hDelta1;
private float hDelta;
  // Use this for initialization
  void Start () {
    
    hDelta1 = 0;
  }
  
  // Update is called once per frame
  void Update () {
      // Read the user input
    var x = CrossPlatformInputManager.GetAxis("Mouse X");
    //var y = CrossPlatformInputManager.GetAxis("Mouse Y");

   // print( x );
    angle = (Input.mousePosition.x / Screen.width) - .5f;//rotateSizeX * x;
    angle *= 90;





    hDelta1 = (Input.mousePosition.y / Screen.height) - .5f;

    hDelta = hDelta1; //Mathf.Lerp( hDelta , hDelta1 , Time.deltaTime * rotSpeed );

    target = subject.position  + Vector3.up * (height - hDelta * verticleDelta);
    transform.position = Vector3.Lerp( transform.position , target , moveSpeed * Time.deltaTime ) ;//subject.position  + Vector3.up * (height - hDelta * 3);

    toRotate.localPosition = Vector3.forward * radius;

    angle = Mathf.Clamp( angle , -90 , 90 );

    targetRot = Quaternion.Euler(0, subject.eulerAngles.y + angle + 180 , 0);
    
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,  rotSpeed*Time.deltaTime);


    cameraHolder.rotation = Quaternion.Slerp( cameraHolder.rotation, Quaternion.LookRotation( subject.position - cameraHolder.position ), lookAtSpeed * Time.deltaTime );
    camera.localRotation = Quaternion.Slerp( camera.localRotation , Quaternion.identity , lookAtSpeed * Time.deltaTime );
    camera.localPosition = Vector3.Lerp( camera.localPosition , Vector3.zero  , lookAtSpeed * Time.deltaTime );
  


   // angle *= .95f;
  
  }
}
