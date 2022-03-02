using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
  private Vector3 deltaPosition = Vector3.zero;
  private Transform targetTransform;
  private CharacterController controller;

  private Vector3 gravity = new Vector3(0, -9.81f, 0);
  private Vector3 fallVelocity;

  public bool gravityEnabled = true;

  [HideInInspector]
  public float colliderRadius = 0.5f;

  private void Start() {
    targetTransform = this.transform;
    controller = targetTransform.GetComponent<CharacterController>();
    if (controller == null) {
      controller = targetTransform.gameObject.AddComponent<CharacterController>();
      controller.center = new Vector3(0, 1.08f, 0);
      controller.radius = colliderRadius;
    }
  }

  public void setPosition(Vector3 deltaPosition) {    
    if (controller == null) return;
    deltaPosition.y = 0f;
    controller.Move(targetTransform.rotation * deltaPosition);
  }

  public void setAbsolutePosition(Vector3 position) {
    if (controller == null) return;
    controller.Move(position - targetTransform.position);
  }

  public void move(Vector2 movement, Quaternion referenceRotation) {
    if (controller == null) return;
    Vector3 velocity = referenceRotation * new Vector3(movement.x, 0, movement.y);
    controller.Move(velocity * Time.deltaTime);
  }

  private void Update() {
    if (controller == null) return;
    if (!gravityEnabled) return;
    controller.detectCollisions = false;
    if (controller.isGrounded && fallVelocity.magnitude > 0) {
      fallVelocity = Vector3.zero;
    }

    fallVelocity += gravity * Time.deltaTime;
    controller.SimpleMove(fallVelocity);
  }
}