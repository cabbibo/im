using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookObject : MonoBehaviour {


  public Collider collider;

  public MeshRenderer renderer;

  public Transform lockObj;
  public float offsetVal;

  public void TransferParent(){
  
    transform.parent = lockObj.parent.transform;

     transform.localRotation = lockObj.localRotation;
     transform.localPosition = lockObj.localPosition;
     // transform.localPosition = -Vector3.forward * (renderer.transform.localScale.x);//lockObj.localPosition;
 
  }

  public void Show(){
    renderer.enabled = true;
  }


  public void Update(){

         transform.localRotation = lockObj.localRotation;
     transform.localPosition = lockObj.localPosition;
}
}
