using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMenu : MonoBehaviour {
  private GameObject menuComponents;
  private MovementActions mv;
  private DebouncableTrigger menuButton = new DebouncableTrigger(0.1f, 0.9f);
  private Dictionary<string, Button> buttons = new Dictionary<string, Button>();
  private bool menuActive = false;

  private void Start() {
    menuComponents = transform.Find("MenuComponents").gameObject;
    menuComponents.SetActive(menuActive);
    
    for (int i = 0; i < menuComponents.transform.childCount; i++) {
      Transform menuComponentChild = menuComponents.transform.GetChild(i);
      if (menuComponentChild.name.Contains("Button")) {
        buttons.Add(menuComponentChild.name, menuComponentChild.GetComponent<Button>());
      }
    }
  }

  void Awake() {
    mv = new MovementActions();
  }

  private void OnEnable() {
    mv.Enable();
    EventManager.startListening("player menu pressed", buttonPressed);
    EventManager.startListening("player menu released", buttonReleased);
  }

  private void OnDisable() {
    EventManager.stopListening("player menu pressed", buttonPressed);
    EventManager.stopListening("player menu released", buttonReleased);
  }

  private void SetButtonEmission(string name, bool state) {
    Button button;
    if (buttons.TryGetValue(name + "Button", out button)) {
      button.setEmission(state);
    }
  }

  public void buttonPressed(string param) {
    SetButtonEmission(param, true);
  }

  public void buttonReleased(string param) {
    SetButtonEmission(param, false);
    ParseButton(param);
  }

  private void ParseButton(string param) {
    switch (param) {
      case "exit":
        Quit();
        break;
    }
  }

  private void Quit() {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  }

  private void Update() {
    float menuPressed = mv.Player.menu.ReadValue<float>();

    if (menuButton.update(menuPressed) && menuButton.get()) {
      menuActive = !menuActive;
      menuComponents.SetActive(menuActive);
    }
  }
}
