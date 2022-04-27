using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour {
  private int screenMaterialIndex = 1;
  public Color reservedColor = Color.red;
  private Color defaultColor;
  private bool isReserved = false;
  private bool reservationChanged = false;
  private string eventId;

  private void Start() {
    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    Material screenMaterial = meshRenderer.materials[screenMaterialIndex];
    defaultColor = screenMaterial.color;
  }

  private void OnEnable() {
    eventId = string.Format("{0} reserved", transform.parent.name.ToLower());
    EventManager.startListening(eventId, UpdateReservation);
  }

  private void OnDisable() {
    EventManager.stopListening(eventId, UpdateReservation);
  }

  private void SetReserved() {
    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    Material screenMaterial = meshRenderer.materials[screenMaterialIndex];
    if (isReserved) {
      screenMaterial.SetColor("_EmissionColor", reservedColor * 5f);
      screenMaterial.color = reservedColor;
    } else {
      screenMaterial.SetColor("_EmissionColor", Color.white * (-10f));
      screenMaterial.color = defaultColor;
    }
  }

  private void UpdateReservation(string param) {
    isReserved = param.Equals("1") ? true : false;
    reservationChanged = true;
  }

  private void Update() {
    if (reservationChanged) {
      reservationChanged = false;
      SetReserved();
    }
  }
}
