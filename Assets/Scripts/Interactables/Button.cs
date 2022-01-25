using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
  private Vector3 localRestPosition;
  private Material material;
  public string buttonName;

  void Start() {
    localRestPosition = transform.localPosition;
    material = gameObject.GetComponent<MeshRenderer>().material;
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

    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
    material.EnableKeyword("_EMISSION");
    gameObject.SendMessageUpwards("Button_" + buttonName, true, SendMessageOptions.DontRequireReceiver);
  }

  private void OnTriggerExit(Collider other) {
    if (!validTrigger(other.tag)) {
      return;
    }

    transform.localPosition = localRestPosition;
    material.DisableKeyword("_EMISSION");
    gameObject.SendMessageUpwards("Button_" + buttonName, false, SendMessageOptions.DontRequireReceiver);
  }

  void Update() {

  }
}
