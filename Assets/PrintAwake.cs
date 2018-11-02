// The PrintAwake script is placed on a GameObject.  The Awake function is
// called when the GameObject is started at runtime.  The script is also
// called by the Editor.  An example is when the scene is changed to a
// different scene in the Project window.
// The Update() function is called, for example, when the GameObject transform
// position is changed in the Editor.

using UnityEngine;

[ExecuteInEditMode]
public class PrintAwake : MonoBehaviour
{
    void Awake()
    {
//        Debug.Log("Editor causes this Awake");
    }

        void Start()
    {
  //      Debug.Log("Editor causes this Start");
    }


    void Update()
    {
       // Debug.Log("Editor causes this Update");
    }

    void FixedUpdate()
    {
    //    Debug.Log("Editor causes this FixedUpdate");
    }

    void OnRenderObject(){
//      Debug.Log("Editor causes thsi wow");
    }
}