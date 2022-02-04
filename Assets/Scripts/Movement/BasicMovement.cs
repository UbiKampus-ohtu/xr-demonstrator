using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {
  private MovementActions movementActions;
  private Translate playerTranslation;
  private Turn playerTurn;
  private Tilt playerTilt;
  private XRInputs xrInputs;
  private bool xrEnabled = false;
  private Vector3 hmdPreviousPosition;
  public PlayerSettings settings;
  Transform offset;
  Transform viewport;

  private void Start() {
    playerTranslation = gameObject.AddComponent<Translate>();
    playerTurn = gameObject.AddComponent<Turn>();
    playerTilt = gameObject.AddComponent<Tilt>();
    
    offset = this.transform.Find("Offset");
    viewport = offset.Find("Viewport");

    playerTilt.setTarget(offset);
    playerTranslation.colliderRadius = settings.colliderRadius;

    if (UnityEngine.XR.XRSettings.enabled) {
      xrEnabled = true;
      playerTilt.enabled = false;
      xrInputs = gameObject.AddComponent<XRInputs>();
      hmdPreviousPosition = xrInputs.getHMDLocalPosition();
    } else {
      offset.transform.position += new Vector3(0f, settings.playerEyelineHeight, 0f);
    }
  }

  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  void Update() {
    Vector2 movementInput = movementActions.Player.move.ReadValue<Vector2>() * settings.walkingSpeed;
    Vector2 rotationInput = movementActions.Player.look.ReadValue<Vector2>() * settings.mouseSensitivity;

    Quaternion referenceRotation = viewport.rotation * Quaternion.Euler(-viewport.rotation.eulerAngles.x, 0, -viewport.rotation.eulerAngles.z);
    playerTranslation.move(movementInput, referenceRotation);
    playerTurn.turn(rotationInput.x);
    playerTilt.tilt(-rotationInput.y);

    if (xrEnabled) {
      viewport.localRotation = xrInputs.getHMDLocalRotation();
      viewport.localPosition = xrInputs.getHMDLocalPosition();
      //replace localposition with deltamovement
      //the xr view will drift from the capsule collider if the user moves
    }
  }
}
