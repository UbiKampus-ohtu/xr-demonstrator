using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMenu : MonoBehaviour {
  private GameObject menuComponents;
  private void Start() {
    menuComponents = transform.Find("MenuComponents").gameObject;
    menuComponents.SetActive(false);
  }

  private void OnEnable() {
    EventManager.startListening("player menu", buttonPressed);
  }

  private void OnDisable() {
    EventManager.stopListening("player menu", buttonPressed);
  }

  public void buttonPressed(string param) {
    Debug.Log("Hei Maailma!");
  }
}
