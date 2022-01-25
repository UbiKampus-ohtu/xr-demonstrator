using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
  private Vector3 deltaPosition = Vector3.zero;
  private Transform targetTransform;
  private CharacterController controller;

  private Vector3 gravity = new Vector3(0, -9.81f, 0);
  private Vector3 velocity;

  private void Start() {
    targetTransform = this.transform;
    controller = targetTransform.GetComponent<CharacterController>();
    if (controller == null) {
      controller = targetTransform.gameObject.AddComponent<CharacterController>();
      controller.center = new Vector3(0, 1f, 0);
      controller.radius = 0.5f;
    }
  }

  //use this to move the player with an xr device
  public void setPosition(Vector3 deltaPosition) {
    deltaPosition.y = 0f;
    controller.Move(targetTransform.rotation * deltaPosition);
  }

  public void move(Vector2 movement) {
    Vector3 movement3 = targetTransform.rotation * new Vector3(movement.x, 0, movement.y);
    controller.Move(movement3 * Time.deltaTime);
  }

  private void Update() {
    controller.detectCollisions = false;
    if (controller.isGrounded && velocity.magnitude > 0) {
      velocity = Vector3.zero;
    }

    velocity += gravity * Time.deltaTime;
    controller.SimpleMove(velocity);
  }
}