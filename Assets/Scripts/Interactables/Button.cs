using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
  private Vector3 localRestPosition;
  private Material material;
  private bool released = true;
  public Color color;
  public Color emissive;
  public string eventName = "button";
  public string buttonName = "0";
  public float travel = 0.1f;
  public Vector3 buttonMovementDirection;

  void Start() {
    localRestPosition = transform.localPosition;
    material = gameObject.GetComponent<MeshRenderer>().material;
    material.color = color;
    material.SetColor("_EmissionColor", emissive);
    if (buttonMovementDirection.magnitude == 0) {
      buttonMovementDirection = Vector3.forward;
    } else {
      buttonMovementDirection = Vector3.Normalize(buttonMovementDirection);
    }
  }

  private bool validTrigger(string tag) {
    if (!tag.Equals("Finger")) {
      return false;
    }
    return true;
  }

  private void OnTriggerEnter(Collider other) {
    if (!validTrigger(other.tag)) {
      return;
    }

    if (other.gameObject.name.Equals("fingertip") && released) {
      transform.localPosition += transform.localRotation * buttonMovementDirection * travel;
      EventManager.trigger(eventName + " pressed", buttonName);
      released = false;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (!validTrigger(other.tag)) {
      return;
    }

    if (other.gameObject.name.Equals("fingertipExit")) {
      transform.localPosition = localRestPosition;
      EventManager.trigger(eventName + " released", buttonName);
      released = true;
    }
  }

  public void setEmission(bool state) {
    if (state) {
      material.EnableKeyword("_EMISSION");
    } else {
      material.DisableKeyword("_EMISSION");
    }
  }

  void Update() {

  }
}
