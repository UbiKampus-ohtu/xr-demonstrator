using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
  public Color color;
  public Color emissive;

  public float travel = 0.1f;
  public Vector3 buttonMovementDirection;

  public string eventName = "button";
  public string buttonName = "0";

  private bool released = true;
  private Transform buttonRoot;
  private Vector3 localRestPosition;
  private Material lightBulbMaterial;

  private void initLightBulbMaterial() {
    GameObject lightBulb = buttonRoot.Find("Light").gameObject;
    if (lightBulb != null) {
      lightBulbMaterial = lightBulb.GetComponent<MeshRenderer>().material;
      lightBulbMaterial.color = color;
      lightBulbMaterial.SetColor("_EmissionColor", emissive);
    } 
  }

  private void initButtonMaterial() {
    GameObject button = buttonRoot.Find("Button").gameObject;
    if (button != null) {
      Material buttonMaterial = button.GetComponent<MeshRenderer>().material;
      buttonMaterial.color = color;
    }
  }

  void Start() {
    buttonRoot = transform.Find("PushButton");
    if (buttonRoot == null) {
      buttonRoot = this.transform;
    }
    localRestPosition = buttonRoot.localPosition;
    
    initLightBulbMaterial();
    initButtonMaterial();

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
      buttonRoot.localPosition += buttonRoot.localRotation * buttonMovementDirection * travel;
      EventManager.trigger(eventName + " pressed", buttonName);
      released = false;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (!validTrigger(other.tag)) {
      return;
    }

    if (other.gameObject.name.Equals("fingertipExit")) {
      buttonRoot.localPosition = localRestPosition;
      EventManager.trigger(eventName + " released", buttonName);
      released = true;
    }
  }

  public void setEmission(bool state) {
    if (state) {
      lightBulbMaterial.EnableKeyword("_EMISSION");
    } else {
      lightBulbMaterial.DisableKeyword("_EMISSION");
    }
  }

  void Update() {

  }
}
