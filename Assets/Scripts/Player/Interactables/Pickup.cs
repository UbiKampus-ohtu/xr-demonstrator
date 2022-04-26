using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
  public bool precisionTool = false;
  public Vector3 offset = Vector3.zero;
  
  private bool pickedUp = false;
  private Transform parent;

  public void take(Transform parent) {
    pickedUp = true;
    this.parent = parent;
  }

  public void drop() {
    pickedUp = false;
    parent = null;
  }

  private void LateUpdate() {
    if (!pickedUp || parent == null) return;

    transform.position = parent.position;
    transform.rotation = parent.rotation;

  }
}
