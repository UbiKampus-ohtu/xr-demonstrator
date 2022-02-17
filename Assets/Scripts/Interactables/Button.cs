using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
  public Color color;
  public Color emissive;

  public float travel = 0.1f;
  private Vector3 buttonMovementDirection = new Vector3(0, -1, 0);

  public string eventName = "button";
  public string buttonName = "0";

  public AudioClip buttonPressSound;
  public AudioClip buttonDepressSound;

  private bool released = true;
  private Transform buttonRoot;
  private Vector3 localRestPosition;
  private Material lightBulbMaterial;
  public Material lightBulbMaterialActive;
  private AudioSource audioSource;

  private MeshRenderer lightbulbRenderer;
  private MeshRenderer buttonRenderer;

  private void initLightBulbMaterial() {
    GameObject lightBulb = buttonRoot.Find("Light").gameObject;
    if (lightBulb != null) {
      lightbulbRenderer = lightBulb.GetComponent<MeshRenderer>();
      lightBulbMaterial = lightbulbRenderer.material;
      lightBulbMaterial.color = color;
    } 
  }

  private void initButtonMaterial() {
    GameObject button = buttonRoot.Find("Button").gameObject;
    if (button != null) {
      buttonRenderer = button.GetComponent<MeshRenderer>();
      Material buttonMaterial = buttonRenderer.material;
      buttonMaterial.color = color;
    }
  }

  void Start() {
    audioSource = gameObject.GetComponent<AudioSource>();

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

  private void playSound(bool isDown) {
    if (isDown) {
      audioSource.clip = buttonPressSound;
    } else {
      audioSource.clip = buttonDepressSound;
    }
    audioSource.Play();
  }

  private void OnTriggerEnter(Collider other) {
    if (!validTrigger(other.tag)) {
      return;
    }

    if (other.gameObject.name.Equals("fingertip") && released) {
      buttonRoot.localPosition += buttonRoot.localRotation * buttonMovementDirection * travel;
      EventManager.trigger(eventName + " pressed", buttonName);
      playSound(true);
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
      playSound(false);
      released = true;
    }
  }

  public void setEmission(bool state) {
    if (state) {
      lightbulbRenderer.material = lightBulbMaterialActive;
      lightbulbRenderer.material.SetColor("_EmissionColor", color * 5f);
    } else {
      lightbulbRenderer.material = lightBulbMaterial;
    }
  }
}
