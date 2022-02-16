using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {
  public string eventName;
  public string lightId;
  
  private Light thisLight;

  private void Start() {
    thisLight = GetComponent<Light>();
    thisLight.enabled = false;
  }

  //registers to eventmanager on gameobject activation. add " pressed" to eventname to trigger on button press, " released" to trigger on release
  private void OnEnable() {
    EventManager.startListening(eventName + " pressed", toggleLight);
  }

  //removes registration to eventmanager on gameobject deactivation
  private void OnDisable() {
    EventManager.stopListening(eventName + " pressed", toggleLight);
  }

  //method that is called as a callback, when the event name is triggered in eventmanager
  public void toggleLight(string param) {
    Debug.LogFormat("lightId: {0} paramId:{1}", lightId, param);

    // the action is triggered, if the param equals assigned lightId, or if the param is emptry (any assumed in this case)
    if (param.Equals("") || param.Equals(lightId)) {
      thisLight.enabled = !thisLight.enabled;
    } 
  }
}
