﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour {
	private float timer;
	private string eventId;
	private bool movementInRoom;
	private string status;

	private void Start() {
  }

  private void OnEnable() {
		movementInRoom = false;
		eventId = string.Format("{0} motionSensor", gameObject.name);
    EventManager.startListening(eventId, movementSensed);
  }

	private void OnDisable() {
		EventManager.stopListening(eventId, movementSensed);
	}

	private void Update() {
		timer += Time.deltaTime;

		if (timer % 5 <= 0.01f && movementInRoom) {
			roomPopulationChange();
			movementInRoom = false;
			timer = 0;

		} else if (timer >= 360 && !movementInRoom) {
			print("timer " + timer);
			movementInRoom = false;
			roomPopulationChange();
			timer = 0;
		}

  }
  
	private void movementSensed(string param) {
		movementInRoom = true;
	}

	private void roomPopulationChange() {
		if (movementInRoom) {
			status = "on";
		} else {
			status = "off";
		}

		EventManager.trigger("change material", status);
		//EventManager.trigger("change text", status);
	}
}