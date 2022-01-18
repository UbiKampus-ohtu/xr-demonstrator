using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
  private Vector3 deltaPosition = Vector3.zero;
  private Transform targetTransform;

  private void Start() {
    targetTransform = this.transform;
  }

  public void move(Vector2 movement) {
    deltaPosition = new Vector3(movement.x, 0f, movement.y);
  }

  private bool newPositionIsValid(Vector3 position) {
    // Add collision checks here; If the new position causes collision in parent, return false
    return true;
  }

  private void Update() {
    Vector3 newPosition = targetTransform.position + targetTransform.rotation * deltaPosition;
    if (newPositionIsValid(newPosition)) {
      targetTransform.position = newPosition;
    }
  }
}