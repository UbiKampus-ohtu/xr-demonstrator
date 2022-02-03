using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHand : MonoBehaviour {
  private Animator animator;

  private void Start() {
    animator = GetComponentInChildren<Animator>();
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
