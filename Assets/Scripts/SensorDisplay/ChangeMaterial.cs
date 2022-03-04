using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour {
	private Color c = new Color(27f, 79f, 191f);

	private void OnEnable() {
		EventManager.startListening("change material", emissionSwitch);
	}

	private void emissionSwitch(string param) {
		if (param.Equals("on")) {
			GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", c*0.03f);
		} else if (param.Equals("off")) {
			GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", c*0.002f);
		}
	}

}