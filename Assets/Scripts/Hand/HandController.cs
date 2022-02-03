using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
  private MovementActions movementActions;
  private HandContext context;

  private void Start() {
    context = GetComponent<HandContext>();
  }

  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  private bool snapDecounbed = true;
  private bool grabDebounced = true;
  private void Update() {
    float snapping = movementActions.Player.click.ReadValue<float>();
    float grabbing = movementActions.Player.grab.ReadValue<float>();

    if (grabbing > 0.5f && grabDebounced) {
      context.grab(true);
      grabDebounced = false;
    } else if (grabbing <= 0.5f && !grabDebounced) {
      grabDebounced = true;
      context.grab(false);
    }

    if (snapping > 0.5f && snapDecounbed) {
      context.snap(true);
      snapDecounbed = false;
    } else if (snapping <= 0.5f && !snapDecounbed) {
      snapDecounbed = true;
      context.snap(false);
    }
  }
}
