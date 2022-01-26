using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {
  private MovementActions movementActions;
  private Translate playerTranslation;
  private Turn playerTurn;
  private Tilt playerTilt;
  private XRInputs xrInputs;
  public bool xrEnabled = false;
  private Vector3 hmdPreviousPosition;
  public PlayerSettings settings;
  Transform offset;

  private void Start() {
    playerTranslation = gameObject.AddComponent<Translate>();
    playerTurn = gameObject.AddComponent<Turn>();
    playerTilt = gameObject.AddComponent<Tilt>();
    
    offset = this.transform.Find("Offset");
    offset.transform.position += new Vector3(0f, 1.6f, 0f);
    playerTilt.setTarget(offset);

    if (xrEnabled && UnityEngine.XR.XRSettings.enabled) {
      xrEnabled = true;
      playerTilt.enabled = false;
      xrInputs = gameObject.AddComponent<XRInputs>();
      hmdPreviousPosition = xrInputs.getHMDLocalPosition();
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

    Quaternion referenceRotation = offset.rotation * Quaternion.Euler(-offset.rotation.eulerAngles.x, 0, 0);
    playerTranslation.move(movementInput, referenceRotation);
    playerTurn.turn(rotationInput.x);
    playerTilt.tilt(-rotationInput.y);

    if (xrEnabled) {
      offset.localRotation = xrInputs.getHMDLocalRotation();
      Vector3 hmdLocalPosition = xrInputs.getHMDLocalPosition();
      Vector3 hmdDeltaPosition = hmdLocalPosition - hmdPreviousPosition;

      hmdDeltaPosition.y = 0f;
      hmdPreviousPosition = hmdLocalPosition;
      offset.localPosition = new Vector3(0f, hmdLocalPosition.y, 0f);
      playerTranslation.setPosition(hmdDeltaPosition);
    }    
  }
}
