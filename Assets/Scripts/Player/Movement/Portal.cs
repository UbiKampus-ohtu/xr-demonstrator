using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
  public GameObject target;
  [HideInInspector]
  public Portal targetPortal;
  [HideInInspector]
  public bool isEnabled = true;

  private void Start() {
    if (target == null) return;
    targetPortal = target.GetComponent<Portal>();
  }
}
