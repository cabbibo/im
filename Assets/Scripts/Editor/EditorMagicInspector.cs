using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if (UNITY_EDITOR) 

[CustomEditor(typeof(EditorMagic))]
public class EditorMagicInspector : Editor {
 
  private EditorMagic creator;
  

  private void OnEnable () {
    creator = target as EditorMagic ;
  }

  public override void OnInspectorGUI () {
    DrawDefaultInspector();
    if(GUILayout.Button("Create")){ creator.Create();  } 
    if(GUILayout.Button("Destroy")){ creator.Destroy();  } 
  }

}
#endif