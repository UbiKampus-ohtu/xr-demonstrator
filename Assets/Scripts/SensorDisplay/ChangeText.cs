﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {
    public Text StatusText;

    private void OnEnable() {
        EventManager.startListening("change text", changeText);
    }

    private void OnDisable() {
        EventManager.stopListening("change text", changeText);
    }

    private void changeText(string param) {
        if (param.Equals("red")) {
			StatusText.text = "Swarm";
		} else if (param.Equals("yellow")) {
			StatusText.text = "Horde";
		} else if (param.Equals("green")) {
			StatusText.text = "Pack";
		} else if (param.Equals("blue")) {
			StatusText.text = "Few";
		} else {
            StatusText.text = "None";
        }
    }
}