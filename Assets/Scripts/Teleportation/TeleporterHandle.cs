using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterHandle : MonoBehaviour {
  private Animator animator;

  private void Start() {
    animator = GetComponent<Animator>();
  }

  public void exit() {
    active = false;
  }

  public void setValid(bool state) {
    valid = state;
  }

  private bool active = true;
  private bool valid = true;

  private void Update() {
    animator.SetBool("active", active);
    animator.SetBool("valid", valid);
  }
}
