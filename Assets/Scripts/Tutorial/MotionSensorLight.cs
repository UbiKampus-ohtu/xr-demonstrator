using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorLight : MonoBehaviour {
  private string eventId;

  private float lightIntensity;
  public Light motionSensorLight;

  private void Start() {
    lightIntensity = motionSensorLight.intensity;
  }

  private void OnEnable() {
    eventId = string.Format("{0} motionSensor", gameObject.name.ToLower());
    EventManager.startListening(eventId, MotionTrigger);
  }

  private void OnDisable() {
    EventManager.stopListening(eventId, MotionTrigger);
  }

  private void MotionTrigger(string param) {
    lightIntensity = 2f;
  }

  private void Update() {
    if (lightIntensity > 0.1f) {
      lightIntensity -= 0.01f;
      motionSensorLight.intensity = lightIntensity;
    }
  }
}
