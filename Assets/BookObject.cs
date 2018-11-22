using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookObject : MonoBehaviour {


  public Collider collider;

  public MeshRenderer renderer;

  public Transform lockObj;

  public void Hide(){
   // renderer.enabled = false;

        transform.parent = lockObj.parent.transform;

    print(lockObj.parent);
    transform.localPosition = lockObj.localPosition;
    transform.localRotation = lockObj.localRotation;
    transform.localScale = lockObj.localScale;
  

  }

  public void Show(){
    renderer.enabled = true;



  }

}
