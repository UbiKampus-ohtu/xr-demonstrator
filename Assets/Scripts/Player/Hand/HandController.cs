using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
  private MovementActions movementActions;
  private GenericHand hand;
  private HandContext context;
  private XRInputs xrInputs;

  public bool leftHand = false;

  private DebouncableTrigger snapTrigger = new DebouncableTrigger(0.49f, 0.5f);
  private DebouncableTrigger grabTrigger = new DebouncableTrigger(0.1f, 0.9f);

  private void Start() {
    hand = GetComponent<GenericHand>();
    context = hand.context;
    xrInputs = gameObject.AddComponent<XRInputs>();
  }

  public static bool isXrEnabled() {
    return XRInputs.isXrEnabled();
  }

  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  private void Update() {
    float snapping = movementActions.Player.click.ReadValue<float>();
    float grabbing = movementActions.Player.grab.ReadValue<float>();

    if (grabTrigger.update(grabbing)) {
      context.grab(grabTrigger.get());
    }

    if (snapTrigger.update(snapping)) {
      context.snap(snapTrigger.get());
    }

    if (xrInputs.xrEnabled) {
      hand.setPosition(xrInputs.getHandLocalPosition(leftHand));
      hand.setRotation(xrInputs.getHandLocalRotation(leftHand));
    }
  }
}
