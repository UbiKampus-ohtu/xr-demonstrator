using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMenu : MonoBehaviour {
  private GameObject menuComponents;
  private MovementActions mv;
  private DebouncableTrigger menuButton = new DebouncableTrigger(0.1f, 0.9f);
  private bool menuActive = false;

  private void Start() {
    menuComponents = transform.Find("MenuComponents").gameObject;
    menuComponents.SetActive(menuActive);
  }

  void Awake() {
    mv = new MovementActions();
  }

  private void OnEnable() {
    mv.Enable();
    EventManager.startListening("player menu", buttonPressed);
  }

  private void OnDisable() {
    EventManager.stopListening("player menu", buttonPressed);
  }

  public void buttonPressed(string param) {
    Debug.Log("Hei Maailma!");
  }

  private void Update() {
    float menuPressed = mv.Player.menu.ReadValue<float>();

    Debug.Log(menuPressed);

    if (menuButton.update(menuPressed) && menuButton.get()) {
      menuActive = !menuActive;
      menuComponents.SetActive(menuActive);
    }
  }
}
