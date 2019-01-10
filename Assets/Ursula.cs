using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ursula : MonoBehaviour {



  public Wand wand;

  public TerrainEngine engine;
  public Animator animator;
  public Transform camera;
  public Transform soul;
  public Transform head;

  public bool locked;
  public Vector3 targetPosition;
  public Quaternion targetRotation;


  public SkinnedMeshRenderer bodyRenderer;
  public SkinnedMeshRenderer shoeRenderer;

  public Color activeColor;
  public Color passiveColor;


  private Vector3 cameraForward;             // The current forward direction of the camera
  private Vector3 m_Move;
  private Vector3 moveTarget;

  public float runForce;
  public float lockForce;
  public float runMultiplier;
  public float toRotLerp;
  public float forceCutoffRadiusStart;
  public float forceCutoffRadiusEnd;

  public float walkCutoffRadiusStart;
  public float walkCutoffRadiusEnd;

  public float trailLerp;
  public float maxDist;
  
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

  private Vector3 oPos;
  private Vector3 deltaPos;
  private Quaternion oRot;
  private Quaternion deltaRot;
  private float lockStartTime;
  private float lockSpeed;

  private Page targetPage;

  private Vector3 tmpPos;
  private Quaternion tmpRot;

  public Life bodyParticlesSet;

  public bool bookRaised;
  public BookLock bookLock;


	// Use this for initialization
	void Start () {

    lockStartTime = 0;
    oRot = Quaternion.identity;
    deltaRot = Quaternion.identity;
    oPos = Vector3.zero;
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
   
    trailPos1 = Vector3.Lerp( trailPos1 , position , trailLerp );
    trailPos2 = Vector3.Lerp( trailPos2 , trailPos1 , trailLerp );
    trailPos3 = Vector3.Lerp( trailPos3 , trailPos2 , trailLerp );
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



public void SetGrounded(){
  if( gravity == false ){

    gravity = true;
   // animator.

    print("hellllooo");

    if( animator.GetCurrentAnimatorStateInfo(0).IsName("Sleeping"))
     {
      print("hi2");
      animator.Play("Grounded");
     }

    if( animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
     {
      print("hi3");
      animator.Play("Grounded");
     }

       if( animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
     {

      print("fall1");

      animator.SetTrigger("FallToGrounded");
     }



  }
}


/*
  
  Movement Stuff

*/

  void DoMovement(){


    oPos = transform.position;
    oRot = transform.rotation;

    force = Vector3.zero;
    if( locked == false ){

      //WASDMove();


      if( moveTarget != null ){
      FreeMoveToPosition( moveTarget );
}

    force += m_Move * moveForce * .1f  * runForce;

    //deltaRot = transform.rotation - oRot;

    }else{


     /* float lerpVal = Mathf.Clamp( (Time.time - lockStartTime) / lockSpeed ,  0,1);

      lerpVal = lerpVal * lerpVal * (3 - 2 * lerpVal);
      transform.position = Vector3.Lerp( tmpPos , targetPage.subjectTarget.position  , lerpVal );
      transform.rotation = Quaternion.Slerp( tmpRot ,targetPage.subjectTarget.rotation,lerpVal);


    deltaPos = (transform.position - oPos);
   // deltaRot = transform.rotation - oRot;

     Vector3 m = transform.InverseTransformDirection(deltaPos);
      float turn = Mathf.Atan2(m.x, m.z);
      float forward = 1000*m.z;

      //animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
      animator.SetFloat("Forward", forward * Time.deltaTime, 0.1f, Time.deltaTime);*/



        MoveToPosition( targetPage.subjectTarget.position );

        deltaRot = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation ,targetPage.subjectTarget.rotation,toRotLerp );

        //angle = Quaternion.Angle( transform.rotation, deltaRot %3.14f );
        //Todo: Make a way so that we rotate properly!
        SetMoveTarget( targetPage.subjectTarget.position );


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



      velocity += force;
      velocity *= dampening;


      transform.position += velocity;//  * .001f;//m_Move  * .3f* speed;


      Vector3 m = transform.InverseTransformDirection(velocity);
      float turn = Mathf.Atan2(m.x, m.z);
      float forward = m.z;


      if( locked == false ){

        Rotate(forward , turn);
        animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
      }else{
        animator.SetFloat("Turn", 0, 0.1f, Time.deltaTime);
      }


      animator.SetFloat("Forward", forward*runMultiplier, 0.1f, Time.deltaTime);



      deltaPos = transform.position - oPos;





    //m_Move = transform.InverseTransformDirection(m_Move);
  }

  public void LookAtBook(){

    if( bookRaised ){
      LowerBook();
    }else{
      RaiseBook();
    }
  }

  
  public void RaiseBook(){
    bookRaised = true;
    animator.SetTrigger("RaiseBook");
    bookLock.LockBook();
  }

  public void LowerBook(){
    bookRaised = false;
    animator.SetTrigger("LowerBook");
  bookLock.UnlockBook();
  }


  public void WASDMove(){
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
    
  }


  public void SetMoveTarget(){
    moveTarget = wand.position;
  }

  public void SetMoveTarget(Ray ray){
    moveTarget = wand.position;
  }



  public void SetMoveTarget( Vector3 p ){
    moveTarget = p;
  }


  public void MoveToPosition( Vector3 p ){


      Vector3 dif = p-transform.position;

      float angle = 0;

      bool inside = false;

      if( dif.magnitude > maxDist ){
        transform.position = transform.position + dif.normalized * maxDist; // = dif.normalized * .03f * lockForce;// m_Move * moveForce;
      }

      if( dif.magnitude > forceCutoffRadiusStart ){
        inside = false;
        force = dif.normalized * .03f * lockForce;// m_Move * moveForce;
      }else{

        inside = true;

        if( dif.magnitude > forceCutoffRadiusEnd  ){
          force = dif.normalized * .03f * lockForce  *  (  (dif.magnitude-forceCutoffRadiusEnd)  / (forceCutoffRadiusStart - forceCutoffRadiusEnd)); 
        }


        
        //angle = Quaternion.Angle( transform.rotation, deltaRot %3.14f );
        //Todo: Make a way so that we rotate properly!

      }

  }


    public void FreeMoveToPosition( Vector3 p ){


      Vector3 dif = p-transform.position;

      float angle = 0;

      bool inside = false;

      if( dif.magnitude > maxDist ){
        transform.position = transform.position + dif.normalized * maxDist; // = dif.normalized * .03f * lockForce;// m_Move * moveForce;
      }

      if( dif.magnitude > walkCutoffRadiusStart ){
        inside = false;
        force = dif.normalized * .03f * lockForce;// m_Move * moveForce;
      }else{

        inside = true;

        if( dif.magnitude > walkCutoffRadiusEnd  ){
          force = dif.normalized * .03f * lockForce  *  (  (dif.magnitude-walkCutoffRadiusEnd)  / (walkCutoffRadiusStart - walkCutoffRadiusEnd)); 
        }


        
        //angle = Quaternion.Angle( transform.rotation, deltaRot %3.14f );
        //Todo: Make a way so that we rotate properly!

      }

  }



public void Emit(){

    bodyParticlesSet.shader.SetFloat("_Emit" , 1 );
    bodyParticlesSet._WhileLiving(1);
    bodyParticlesSet.shader.SetFloat("_Emit" , 0 );
  
}


  public void Lock( Page p , float speed ){

    lockSpeed = speed;


    targetPage = p;
    locked = true;
    lockStartTime = Time.time;

    tmpPos = transform.position;
    tmpRot = transform.rotation;
  }

  public void Unlock(){
    locked = false;
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
