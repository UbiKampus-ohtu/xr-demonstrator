using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRDependent : MonoBehaviour {
  void Start() {
    if (!XRInputs.isXrEnabled()) {
      Destroy(gameObject);
    }
  }
}
