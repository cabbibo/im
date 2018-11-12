using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ursula : MonoBehaviour {



  public TerrainEngine engine;
  public Animator animator;
  public Transform camera;


  public Collider bubble;
  public MeshRenderer bubbleRenderer;
  public bool bubbleActive;

  public Color activeColor;
  public Color passiveColor;


  private Vector3 cameraForward;             // The current forward direction of the camera
  private Vector3 m_Move;
  public float speed;

  public bool gravity;

  public Vector3 velocity;
  public Vector3 force;

  public float moveForce;
  public float gravityForce;
  public float dampening;




	// Use this for initialization
	void Start () {
    velocity = Vector3.zero;
		DeactivateBubble();
	}

  /*

  BUBBLE STUFF 

  */

  void OnMouseOver(){
    bubbleRenderer.material.SetColor("_OutlineColor" , activeColor );
    bubbleRenderer.material.SetFloat("_Threshold" , .01f );
    bubbleRenderer.material.SetFloat("_Fade" , .2f );
  }
  
  void OnMouseExit(){
    bubbleRenderer.material.SetColor("_OutlineColor" , passiveColor );
    bubbleRenderer.material.SetFloat("_Threshold" , .1f );
    bubbleRenderer.material.SetFloat("_Fade" , 3 );
  }



  public void ActivateBubble(){
    bubbleRenderer.enabled = true;
    bubbleActive = true;
  }


    public void DeactivateBubble(){
    bubbleRenderer.enabled = false;
    bubbleActive = false;
  }





        

  // Update is called once per frame
  void Update () {
   
    
  // print( c.r );
   Shader.SetGlobalVector("_Player", transform.position );
   Shader.SetGlobalVector("_Velocity", velocity );
  }

  private void FixedUpdate() {
    DoMovement();   
  }
  


/*
  
  Movement Stuff

*/

  void DoMovement(){


    force = Vector3.zero;
     // read inputs
    float h = CrossPlatformInputManager.GetAxis("Horizontal");
    float v = CrossPlatformInputManager.GetAxis("Vertical");
   // bool crouch = Input.GetKey(KeyCode.C);


    // calculate move direction to pass to character
    if (camera != null)
    {
        // calculate camera relative direction to move:
        cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = (v*cameraForward + h*camera.right);
    }
    else
    {
        // we use world-relative directions in the case of no main camera
        m_Move = v*Vector3.forward + h*Vector3.right;
       
    } 


    float height = engine.SampleHeight( transform.position );

    if( transform.position.y < height +.2f ){

        //force += Vector3.up;

        velocity = new Vector3( velocity.x , 0 , velocity.z )  * .5f;

        transform.position = new Vector3( transform.position.x , height , transform.position.z );
    }else{

      if( gravity ){
        force -= Vector3.up  * gravityForce ;
      }
    }
    

    force += m_Move * moveForce;

    velocity += force;
    velocity *= dampening;


    transform.position += velocity  * .001f;//m_Move  * .3f* speed;



    //m_Move = transform.InverseTransformDirection(m_Move);
    updateAnimator(transform.InverseTransformDirection(velocity));

  }

  void updateAnimator( Vector3 m ){

      float turn = Mathf.Atan2(m.x, m.z);
      float forward = m.z;

      Rotate(forward , turn);

      animator.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
      animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
  }


      void Rotate(float f  , float t)
    {
      // help the character turn faster (this is in addition to root rotation in the animation)
      float turnSpeed = Mathf.Lerp(180, 360, f);
      transform.Rotate(0, t * turnSpeed * Time.deltaTime, 0);
    }



    public void SetGravity(){ gravity = true; }


}
