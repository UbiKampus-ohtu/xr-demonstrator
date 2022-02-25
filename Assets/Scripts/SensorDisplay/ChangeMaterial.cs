using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour {
  public Material MatRed;
	public Material MatYellow;
	public Material MatGreen;
	public Material MatBlue;
	public Material MatGrey;

	private void OnEnable() {
		EventManager.startListening("change material", changeMaterial);
	}

	private void changeMaterial(string param) {
		if (param.Equals("red")) {
			GetComponent<MeshRenderer>().material = MatRed;
		} else if (param.Equals("yellow")) {
			GetComponent<MeshRenderer>().material = MatYellow;
		} else if (param.Equals("green")) {
			GetComponent<MeshRenderer>().material = MatGreen;
		} else if (param.Equals("blue")) {
			GetComponent<MeshRenderer>().material = MatBlue;
		} else if (param.Equals("grey")) {
			GetComponent<MeshRenderer>().material = MatGrey;
		}
	}

}