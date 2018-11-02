using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lens : LifeForm {

  public Material BlitMaterial;
  public Material FaceMaterial;
  public Material BackgroundMaterial;
  public GameObject background;
  public Material skybox;

  public override void Activate(){

    if( BlitMaterial != null ){
      Camera.main.GetComponent<DoBlitPass>().enabled = true;
      Camera.main.GetComponent<DoBlitPass>().material = BlitMaterial;
    }else{
      Camera.main.GetComponent<DoBlitPass>().enabled = false;
    }
    //print( GameObject.FindGameObjectWithTag("FACE").GetComponent<MeshRenderer>());

    MeshRenderer r = GameObject.FindGameObjectWithTag("FACE").GetComponent<MeshRenderer>();
    r.material = FaceMaterial;
    background.SetActive(true);
    RenderSettings.skybox = skybox;
 
  }


  public override void Deactivate(){

    background.SetActive(false);
 
  }


}
