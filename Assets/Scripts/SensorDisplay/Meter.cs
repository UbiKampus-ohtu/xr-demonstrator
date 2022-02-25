﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour {
	private float timer;
	private float timerB;
  private float waitTime;
	private string eventId;
	private Vector3 columnScale;
	private Vector3 columnZchange;
	private Transform columnTransform;
	bool movement;

	private void Start() {
		ChangeMaterial changeMat = GetComponentInChildren<ChangeMaterial>();
		columnTransform = transform.Find("MeterColumn");
  }

  private void OnEnable() {
		columnZchange = new Vector3(0,0,0);
		eventId = string.Format("{0} motionSensor", gameObject.name);
    EventManager.startListening(eventId, prepareScaleChange);
		//EventManager.startListening(eventId, grows);
  }

	private void OnDisable() {
		EventManager.stopListening(eventId, prepareScaleChange);
	}

	private void Update() {
    timer += Time.deltaTime;
		timerB += Time.deltaTime;
		if (columnTransform.localScale.z <= 0.1f) {
			print("TIMEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE: " + timerB);
		}

    if (timer >= 10f) {
			if (movement) {
				growColumn();
			} else {
				shrinkColumn();
			}
      
			statusChange(columnTransform.localScale.z);
			timer = 0;
    }
  }
  
	private void prepareScaleChange(string param) {
		if (columnZchange.z < 1f) {
			columnZchange += new Vector3(0,0,0.01f);
		}
		movement = true;
	} 

	private void shrinkColumn() {
		if (columnTransform.localScale.z > 0.1f) {
			columnScale = columnTransform.localScale -= new Vector3(0, 0, 0.001f);
		}
  }

	private void statusChange(float height) {
		string status = "";
		if (height >= 0.75f) {
			status = "red";
		} else if (height >= 0.5f) {
			status = "yellow";
		} else if (height >= 0.2f) {
			status = "green";
		} else if (height > 0.1f) {
			status = "blue";
		} else {
			status = "grey";
		}
		EventManager.trigger("change material", status);
		EventManager.trigger("change text", status);
	}

	private void growColumn() {
		if (columnZchange.z + columnScale.z < 1f) {
			//columnScale = columnTransform.localScale += new Vector3(0,0,0.01f);
			columnScale = columnTransform.localScale += columnZchange;
		} else {
			columnScale = columnTransform.localScale = new Vector3(columnTransform.localScale.x, columnTransform.localScale.y, 1f);
		}
		movement = false;
		columnZchange = new Vector3(0,0,0);
  }
}