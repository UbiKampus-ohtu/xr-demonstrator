using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour {
  private Transform targetTransform;
  private Quaternion deltaRotation = Quaternion.identity;

  void Start() {
    targetTransform = this.transform;        
  }

  public void turn(float degrees) {
    deltaRotation = Quaternion.Euler(0f, degrees, 0f);
  }

  void Update() {
    targetTransform.rotation *= deltaRotation;
  }
}
