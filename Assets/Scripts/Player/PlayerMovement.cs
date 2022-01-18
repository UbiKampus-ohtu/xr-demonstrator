using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private MovementActions movementActions;
    private Translate playerTranslation;
    private Turn playerTurn;
    private Tilt playerTilt;

    private void Start() {
      Transform offset = this.transform.Find("Offset");
      offset.transform.position += new Vector3(0f, 1.6f, 0f);

      playerTranslation = gameObject.AddComponent<Translate>();
      playerTurn = gameObject.AddComponent<Turn>();
      playerTilt = gameObject.AddComponent<Tilt>();
      playerTilt.setTarget(offset);
    }

    private void Awake() {
      movementActions = new MovementActions();
    }

    private void OnEnable() {
      movementActions.Enable();
    }

    void Update() {
        Vector2 movementInput = 0.00001f * movementActions.Player.move.ReadValue<Vector2>() / Time.deltaTime;
        Vector2 rotationInput = 0.0005f * movementActions.Player.look.ReadValue<Vector2>() / Time.deltaTime;

        playerTranslation.move(movementInput);
        playerTurn.turn(rotationInput.x);
        playerTilt.tilt(-rotationInput.y);
    }
}
