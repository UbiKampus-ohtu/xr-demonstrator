using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour {
  private Transform targetTransform;
  private Quaternion deltaRotation = Quaternion.identity;

  public void setTarget(Transform target) {
    targetTransform = target;
  }

  public void tilt(float degrees) {
    deltaRotation = Quaternion.Euler(degrees, 0f, 0f);
  }

  void Update() {
    if (targetTransform == null) {
      return;
    }
    targetTransform.rotation *= deltaRotation;
  }
}
