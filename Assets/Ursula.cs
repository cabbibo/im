using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ursula : MonoBehaviour {



  public TerrainEngine engine;
  public Animator animator;
  public Transform camera;
  public Transform soul;
  public Transform head;


  public SkinnedMeshRenderer bodyRenderer;
  public SkinnedMeshRenderer shoeRenderer;

  public Color activeColor;
  public Color passiveColor;


  private Vector3 cameraForward;             // The current forward direction of the camera
  private Vector3 m_Move;
  public float speed;

  public bool gravity;

  public Vector3 velocity;
  public Vector3 force;

  // for use when binding!
  public Vector3 soulPosition;
  public Vector3 up;
  public Vector3 position;

  public float moveForce;
  public float gravityForce;
  public float dampening;

  public AudioPlayer audio;
  public AudioClip hoverOver;
  public AudioClip hoverOut;

  public Vector3 trailPos1;
  public Vector3 trailPos2;
  public Vector3 trailPos3;




	// Use this for initialization
	void Start () {
    velocity = Vector3.zero;
    trailPos1 = Vector3.zero;
    trailPos2 = Vector3.zero;
    trailPos3 = Vector3.zero;
	}


  public void HoverOver(){
    bodyRenderer.sharedMaterial.SetFloat("_OutlineExtrusion",.03f);
    shoeRenderer.sharedMaterial.SetFloat("_OutlineExtrusion",.03f);   
    bodyRenderer.sharedMaterial.SetColor("_OutlineColor",activeColor);
    shoeRenderer.sharedMaterial.SetColor("_OutlineColor",activeColor);
    audio.Play(hoverOver, Random.Range(.8f,1.2f));
  }
  
  public void HoverOut(){
    bodyRenderer.sharedMaterial.SetFloat("_OutlineExtrusion",.01f);
    shoeRenderer.sharedMaterial.SetFloat("_OutlineExtrusion",.01f);
    bodyRenderer.sharedMaterial.SetColor("_OutlineColor",passiveColor);
    shoeRenderer.sharedMaterial.SetColor("_OutlineColor",passiveColor);
  
    audio.Play(hoverOut,Random.Range(.8f,1.2f) , .4f);
  }






        

  // Update is called once per frame
  void Update () {
   
    trailPos1 = Vector3.Lerp( trailPos1 , position , .15f );
    trailPos2 = Vector3.Lerp( trailPos2 , trailPos1 , .15f );
    trailPos3 = Vector3.Lerp( trailPos3 , trailPos2 , .15f );
  // print( c.r );
   Shader.SetGlobalVector("_Player", position );
   Shader.SetGlobalVector("_Velocity", velocity );
   Shader.SetGlobalVector("_TrailPos1",trailPos1 );
   Shader.SetGlobalVector("_TrailPos2",trailPos2 );
   Shader.SetGlobalVector("_TrailPos3",trailPos3 );
  }

  private void FixedUpdate() {
    
    DoMovement();   
    UpdateValues();
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


  void Rotate(float f  , float t){

    // help the character turn faster (this is in addition to root rotation in the animation)
    float turnSpeed = Mathf.Lerp(180, 360, f);
    transform.Rotate(0, t * turnSpeed * Time.deltaTime, 0);
  
  }




  void UpdateValues(){

    soulPosition = soul.transform.position;
    position = soul.transform.position;
    up = head.transform.position - soulPosition;

  }

    public void SetGravity(){ gravity = true; }


}
