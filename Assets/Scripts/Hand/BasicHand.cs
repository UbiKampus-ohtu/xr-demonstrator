using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHand : MonoBehaviour {
  private MovementActions movementActions;
  private Vector3 restPosition;
  private float distanceToNextSurface = 0f;
  private LineRenderer lineRenderer;

  void Start() {
    restPosition = transform.localPosition;
    lineRenderer = gameObject.GetComponent<LineRenderer>();
    lineRenderer.enabled = false;
  }
  
  private void Awake() {
    movementActions = new MovementActions();
  }

  private void OnEnable() {
    movementActions.Enable();
  }

  public bool clicked = false;
  public bool debounced = true;
  void Update() {
    clicked = movementActions.Player.click.ReadValue<float>() > 0 ? true : false;
  }

  private void FixedUpdate() {
    RaycastHit hit;
    Vector3 originalLocalPosition = transform.localPosition;
    if (!debounced) {
      transform.localPosition = restPosition;
    }

    distanceToNextSurface = 0f;
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f)) {
      distanceToNextSurface = hit.distance;
    }

    if (distanceToNextSurface > 0.05f) {
      lineRenderer.enabled = true;
      if (clicked) {
        transform.localPosition = restPosition + transform.localRotation * transform.InverseTransformDirection(transform.forward) * (distanceToNextSurface + 0.02f);
        debounced = false;
      } else {
        debounced = true;
      }
    } else {
      lineRenderer.enabled = false;
      transform.localPosition = originalLocalPosition;
      if (clicked && debounced) {
        transform.localPosition = restPosition + new Vector3(0,0,0.05f);
        debounced = false;
      } else if (!clicked && !debounced) {
        debounced = true;
        transform.localPosition = restPosition;
      }
    }
  }
}
