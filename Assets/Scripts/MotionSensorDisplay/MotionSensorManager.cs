using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensorManager : MonoBehaviour {
	private float timer;
	private string eventId;
  private bool hasMovement = false;
	private bool movementInRoom = false;
  private float timeSinceLastMovement = 0;
  private ChangeEmission emissionChanger;
  public bool independent;

  private void Awake() {
    emissionChanger = GetComponentInChildren<ChangeEmission>();
  }

  private void OnEnable() {
    string sensorName = transform.parent != null && !independent ? transform.parent.name : transform.name;
		movementInRoom = false;
		eventId = string.Format("{0} motionSensor", sensorName);
    transform.name = eventId;
    EventManager.startListening(eventId, movementSensed);
  }

	private void OnDisable() {
		EventManager.stopListening(eventId, movementSensed);
	}

	private void Update() {
    if (!hasMovement) return;

		timer += Time.deltaTime;
    if (timer > 1) {
      roomPopulationChange();
      movementInRoom = false;
      timer = 0;
    }

    if (timeSinceLastMovement > 360f) {
      hasMovement = false;
    }
  }
  
	private void movementSensed(string param) {
    if (emissionChanger == null) return;
		movementInRoom = true;
    hasMovement = true;
    timer = 0;
	}

	private void roomPopulationChange() {
    if (emissionChanger == null) return;
		if (movementInRoom) {
      timeSinceLastMovement = 0;
      emissionChanger.EmissionUpdate(true);
		} else {
      timeSinceLastMovement += timer;
			emissionChanger.EmissionUpdate(false);
		}
	}
}