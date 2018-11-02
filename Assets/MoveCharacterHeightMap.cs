using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveCharacterHeightMap : MonoBehaviour {

  public TerrainEngine engine;
    public Animator animator;
  private Transform m_Cam;                  // A reference to the main camera in the scenes transform
  private Vector3 m_CamForward;             // The current forward direction of the camera
  private Vector3 m_Move;
  public float speed;
        
  private void Start(){
    m_Cam = Camera.main.transform;
  }

	// Update is called once per frame
	void Update () {
	 
  float h = engine.SampleHeight( transform.position );
   transform.position = new Vector3( transform.position.x , h , transform.position.z );
  // print( c.r );
   Shader.SetGlobalVector("_Player", transform.position );
	}

  private void FixedUpdate() {

    // read inputs
    float h = CrossPlatformInputManager.GetAxis("Horizontal");
    float v = CrossPlatformInputManager.GetAxis("Vertical");
   // bool crouch = Input.GetKey(KeyCode.C);

    // calculate move direction to pass to character
    if (m_Cam != null)
    {
        // calculate camera relative direction to move:
        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = (v*m_CamForward + h*m_Cam.right);
    }
    else
    {
        // we use world-relative directions in the case of no main camera
        m_Move = v*Vector3.forward + h*Vector3.right;
       
    } 
    transform.position += m_Move  * .3f* speed;
    m_Move = transform.InverseTransformDirection(m_Move);
    updateAnimator(m_Move);

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



}