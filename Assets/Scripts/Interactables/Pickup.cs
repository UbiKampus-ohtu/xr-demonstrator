using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
  public bool precisionTool = false;
  public Vector3 offset = Vector3.zero;
  
  public void take(Transform parent) {
    transform.parent = parent;
    if (precisionTool) {
      transform.localRotation = Quaternion.identity;
      transform.localPosition = offset;
    }
  }

  public void drop() {
    transform.parent = null;
  }
}
