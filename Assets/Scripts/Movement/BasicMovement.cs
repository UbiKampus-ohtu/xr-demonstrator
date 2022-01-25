using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {
    private MovementActions movementActions;
    private Translate playerTranslation;
    private Turn playerTurn;
    private Tilt playerTilt;

    [Header("Movement settings")]
    [Range(1.1f, 2.05f)]
    [Tooltip("m/s")]
    public float walkingSpeed = 1.1f;

    public float mouseSensitivity = 0.5f;

    private void Start() {
      Transform offset = this.transform.Find("Offset");
      offset.transform.position += new Vector3(0f, 1.6f, 0f);

      playerTranslation = gameObject.AddComponent<Translate>();
      playerTurn = gameObject.AddComponent<Turn>();
      playerTilt = gameObject.AddComponent<Tilt>();
      playerTilt.setTarget(offset);
      playerTilt.enabled = !UnityEngine.XR.XRSettings.enabled;
    }

    private void Awake() {
      movementActions = new MovementActions();
    }

    private void OnEnable() {
      movementActions.Enable();
    }

    void Update() {
        Vector2 movementInput = movementActions.Player.move.ReadValue<Vector2>() * walkingSpeed;
        Vector2 rotationInput = movementActions.Player.look.ReadValue<Vector2>() * mouseSensitivity;

        playerTranslation.move(movementInput);
        playerTurn.turn(rotationInput.x);
        playerTilt.tilt(-rotationInput.y);
    }
}
