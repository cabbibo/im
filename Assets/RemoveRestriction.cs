using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RemoveRestriction : MonoBehaviour {

  public Page page;
  public UnityEvent OnRestrictionRemove;

  public void Remove(){

    if( page.living ){
      page.restricted = false;
      OnRestrictionRemove.Invoke();
    }

  }


}
