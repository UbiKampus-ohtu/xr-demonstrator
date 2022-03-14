using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour {
  [HideInInspector]
  public Movement movement;
  public PlayerSettings settings;
  private XRInputs xrInputs;
  private bool xrEnabled = false;

  private MovementActions movementActions;

  private void InitLocalPlayer() {
    movementActions = new MovementActions();
    movementActions.Enable();

    movement = GetComponent<Movement>();
    if (movement == null) {
      movement = gameObject.AddComponent<Movement>();
    }
    movement.settings = settings;

    if (UnityEngine.XR.XRSettings.enabled) {
      movement.xrEnabled = true;
      xrInputs = gameObject.AddComponent<XRInputs>();
      xrEnabled = true;
    }
  }

  public override void OnStartClient() {
    Camera viewport = gameObject.GetComponentInChildren<Camera>();
    if (isLocalPlayer) {
      InitLocalPlayer();
      viewport.enabled = true;
    } else {
      viewport.enabled = false;
      this.enabled = false;
    }
  }

  private void Update() {
    if (!isLocalPlayer) return;

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