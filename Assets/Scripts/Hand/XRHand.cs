using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHand : MonoBehaviour {
  private XRInputs xrInputs;

  private void Start() {
    xrInputs = gameObject.AddComponent<XRInputs>();
  }

  private void Update() {
    transform.localPosition = xrInputs.getHandLocalPosition(false);
    transform.localRotation = xrInputs.getHandLocalRotation(false);
  }
}
