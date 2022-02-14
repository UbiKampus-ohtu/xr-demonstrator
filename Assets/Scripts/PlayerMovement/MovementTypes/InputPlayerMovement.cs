using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayerMovement : PlayerMovement {
  private MovementActions movementActions;
  private XRInputs xrInputs;
  private bool xrEnabled = false;

  public override void Start() {
    base.Start();
    if (UnityEngine.XR.XRSettings.enabled) {
      movement.xrEnabled = true;
      xrInputs = gameObject.AddComponent<XRInputs>();
      xrEnabled = true;
    }
  }

  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  private void Update() {
    if (!movement.initialized) return;
    
    Vector2 movementInput = movementActions.Player.move.ReadValue<Vector2>();
    Vector2 rotationInput = movementActions.Player.look.ReadValue<Vector2>();

    movement.move(movementInput);
    movement.turn(rotationInput.x);

    if (xrEnabled) {
      movement.setViewportRotation(xrInputs.getHMDLocalRotation());
      movement.setViewportPosition(xrInputs.getHMDLocalPosition());
    } else {
      movement.tilt(-rotationInput.y);
    }
  }
}
