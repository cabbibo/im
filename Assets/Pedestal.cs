using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pedestal : MonoBehaviour {

  public Page page;
  public float buffer;
  public float buttonSpacing;
  public float lineSpacing;

  public Transform top;


  public ToggleBool toggle1;
  public ToggleBool toggle2;
  public ToggleBool toggle3;


  public Vector3 up;
  public Vector3 topLeft;
  public Vector3 bottomLeft;
  public Vector3 right;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



    Vector3 t1 = page.frame.topLeft + page.frame.right * buffer - page.frame.up * buffer + .5f * page.frame.normal * buttonSpacing;
    toggle1.transform.position = t1;
    toggle2.transform.position = t1 - page.frame.up * buttonSpacing;
    toggle3.transform.position = t1 - page.frame.up * buttonSpacing * 2;


    toggle1.lr.SetWidth(.03f,.03f);
    toggle2.lr.SetWidth(.03f,.03f);
    toggle3.lr.SetWidth(.03f,.03f);


    toggle1.lr.SetVertexCount(5);
    toggle2.lr.SetVertexCount(5);
    toggle3.lr.SetVertexCount(5);


    toggle1.lr.SetPosition( 0 , toggle1.transform.position );
    toggle1.lr.SetPosition( 1 , toggle1.transform.position - page.frame.right * ( buffer + lineSpacing * 4)  );
    toggle1.lr.SetPosition( 2 , toggle1.transform.position - page.frame.right * ( buffer + lineSpacing * 4) - page.frame.up * page.frame.height   );
    

    toggle1.lr.SetPosition( 3 , top.position - top.right  * top.localScale.x+ top.right * lineSpacing *3 -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 4 );
    toggle1.lr.SetPosition( 4 , top.position                                 -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 4 );


    toggle2.lr.SetPosition( 0 , toggle2.transform.position );
    toggle2.lr.SetPosition( 1 , toggle2.transform.position - page.frame.right * ( buffer + lineSpacing * 3)  );
    toggle2.lr.SetPosition( 2 , toggle2.transform.position - page.frame.right * ( buffer + lineSpacing * 3) - page.frame.up * (page.frame.height - buttonSpacing) );


    toggle2.lr.SetPosition( 3 , top.position - top.right  * top.localScale.x + top.right * lineSpacing * 2 -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 3 );
    toggle2.lr.SetPosition( 4 , top.position                                 -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 3 );

    toggle3.lr.SetPosition( 0 , toggle3.transform.position );
    toggle3.lr.SetPosition( 1 , toggle3.transform.position - page.frame.right * ( buffer + lineSpacing * 2)  );
    toggle3.lr.SetPosition( 2 , toggle3.transform.position - page.frame.right * ( buffer + lineSpacing * 2) - page.frame.up * (page.frame.height - buttonSpacing * 2) );

    toggle3.lr.SetPosition( 3 , top.position - top.right  * top.localScale.x + top.right * lineSpacing  -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 2 );
    toggle3.lr.SetPosition( 4 , top.position                                 -  .5f* top.forward * top.localScale.z + top.forward * lineSpacing * 2 );
    //toggle3.lr.SetPosition( 5 , top.position);

		
	}

  public void OnSelect(){

  }


  public void SetCurrentPedestal(){

    //book.pedestalPAge

  }







}
