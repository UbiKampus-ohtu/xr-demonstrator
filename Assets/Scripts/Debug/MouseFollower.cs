using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {
  private MovementActions movementActions;
  private WhiteBoard whiteboard;
  private float mouseSensitivity = 0.005f;
  
  private void Start() {
    whiteboard = transform.parent.GetComponentInChildren<WhiteBoard>();
  }
  
  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  private bool allowedPosition(Vector3 position) {
    if (position.x < 0 || position.x > 1) {
      return false;
    } else if (position.y > 0 || position.y < -1) {
      return false;
    }
    return true;
  }

  private void draw() {
    whiteboard.draw(transform.localPosition.x, -transform.localPosition.y, Color.black);
  }

  private void move(Vector2 mouse) {
    Vector3 newPosition = transform.localPosition + new Vector3(mouse.x, mouse.y, 0f);
    if (allowedPosition(newPosition)) {
      transform.localPosition = newPosition;
    }
  }

  void Update() {
    Vector2 mouseInput = movementActions.Player.look.ReadValue<Vector2>() * mouseSensitivity;
    move(mouseInput);
    draw();
  }
}
