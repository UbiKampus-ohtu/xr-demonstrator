using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {
  private MovementActions movementActions;
  private Translate playerTranslation;
  private Turn playerTurn;
  private Tilt playerTilt;

  public PlayerSettings settings;

  private void Start() {
    playerTranslation = gameObject.AddComponent<Translate>();
    playerTurn = gameObject.AddComponent<Turn>();
    playerTilt = gameObject.AddComponent<Tilt>();
  
    if (UnityEngine.XR.XRSettings.enabled) {
      playerTilt.enabled = false;
    } else {
      Transform offset = this.transform.Find("Offset");
      offset.transform.position += new Vector3(0f, 1.6f, 0f);
      playerTilt.setTarget(offset);
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

    playerTranslation.move(movementInput);
    playerTurn.turn(rotationInput.x);
    playerTilt.tilt(-rotationInput.y);
  }
}
