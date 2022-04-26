using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenericHand : MonoBehaviour {
  private Animator animator;
  private HandController controller;
  [HideInInspector]
  public HandContext context;

  public bool isLeftHand = false;
  public Vector3 handModelOffset = Vector3.zero;

  public void initialize() {
    animator = GetComponentInChildren<Animator>();
    context = gameObject.AddComponent<HandContext>();
    controller = gameObject.AddComponent<HandController>();
    
    controller.leftHand = isLeftHand;
    context.leftHand = isLeftHand;

    if (HandController.isXrEnabled()) {
      context.handModelOffset = new Vector3(0, -0.07f, -0.146f);
    } else {
      context.handModelOffset = handModelOffset;
      if (isLeftHand) {
        Destroy(gameObject);
      }
    }
  }

  public void resetContext() {
    animator.SetBool("pointing", false);
    animator.SetBool("grabbing", false);
    animator.SetBool("grab ready", false);
    animator.SetBool("precision grab", false);
  }

  public void setContext(string context, bool state) {
    animator.SetBool(context, state);
  }

  public void setPosition(Vector3 localPosition) {
    transform.localPosition = localPosition;
  }

  public void setRotation(Quaternion localRotation) {
    transform.localRotation = localRotation;
  }
}
