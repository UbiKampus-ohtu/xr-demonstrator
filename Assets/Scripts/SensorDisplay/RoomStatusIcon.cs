using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatusIcon : MonoBehaviour {
	private float timer;
	public string motionEventId;
  public string reservationEventId;
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
    sensorName = sensorName.ToLower();
		movementInRoom = false;

		motionEventId = string.Format("{0} motionSensor", sensorName);
    reservationEventId = string.Format("{0} reserved", sensorName);

    transform.name = string.Format("{0} StatusIcon", sensorName);
    EventManager.startListening(motionEventId, movementSensed);
    EventManager.startListening(reservationEventId, reservation);
  }

	private void OnDisable() {
		EventManager.stopListening(motionEventId, movementSensed);
	}

	private void Update() {
    if (!hasMovement && !reservationChanged) return;

    if (reservationChanged) {
      emissionChanger.EmissionUpdate(true);
      reservationChanged = false;
    }
    
    if (reserved) return;

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
  
  private bool reserved = false;
  private bool reservationChanged = false;
  private void reservation(string param) {
    if (param.Equals("1")) {
      reserved = true;
      emissionChanger.SetEmissionColor(new Color(255f, 0, 0));
    } else {
      reserved = false;
      emissionChanger.RestoreDefaultColor();
    }
    reservationChanged = true;
  }

	private void movementSensed(string param) {
    if (emissionChanger == null) return;
		movementInRoom = true;
    hasMovement = true;
    timer = 2;
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