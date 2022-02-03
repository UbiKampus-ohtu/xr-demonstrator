using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContext : MonoBehaviour {
  private GenericHand hand;
  public Vector3 handModelOffset = Vector3.zero;
  private Transform handModelTransform;

  private float handDepth = 0.16f;

  private bool grabReady = false;
  private bool grabbing = false;
  private bool snapping = false;

  private Pickup pickup = null;

  private void Start() {
    hand = GetComponent<GenericHand>();
    handModelTransform = transform.Find("hand");
    handModelTransform.localPosition = handModelOffset;
  }

  private RaycastHit getNearestSurface(float distance) {
    RaycastHit hit;
    LayerMask mask = LayerMask.GetMask("Default");
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask)) {
      return hit;
    }
    return new RaycastHit();
  }

  private void OnTriggerEnter(Collider other) {
    if (grabbing) {
      return;
    }
    if (other.transform.parent.GetComponent<Pickup>() != null) {
      hand.setContext("grab ready", true);
      pickup = other.transform.parent.GetComponent<Pickup>();
      grabReady = true;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (grabbing) {
      return;
    }
    if (grabReady) {
      hand.setContext("grab ready", false);
      grabReady = false;
      pickup = null;
    }
  }

  public void grab(bool state) {
    if (state) {
      if (pickup != null && pickup.precisionTool) {
        hand.setContext("precision grab", true);
      } else {
        hand.setContext("grabbing", true);
      }

      if (pickup != null) {
        pickup.take(handModelTransform);
        grabbing = true;
      }
    } else {
      hand.setContext("precision grab", false);
      hand.setContext("grabbing", false);

      if (pickup != null) {
        pickup.drop();
        grabbing = false;
      }
    }
  }

  public void snap(bool state) {
    if (state) {
      snapping = true;
    } else {
      snapping = false;
      Vector3 newPosition = handModelTransform.localPosition;
      newPosition.z = 0;
      handModelTransform.localPosition = newPosition;
    }
  }

  private void FixedUpdate() {
    RaycastHit nearestSurface = getNearestSurface(1f);

    if (nearestSurface.collider == null) {
      hand.setContext("pointing", false);
      return;
    } else {
      hand.setContext("pointing", true);
    }

    if (snapping) {
      Vector3 newPosition = handModelTransform.localPosition;
      float distance = (nearestSurface.point - transform.position).magnitude - handDepth;
      newPosition.z = distance;
      handModelTransform.localPosition = newPosition;
    }
  }
}
