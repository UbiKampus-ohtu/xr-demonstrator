using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNetworkListener : MonoBehaviour {
  private string roomName;

  private void OnEnable() {
    GameObject world = GameObject.Find("/World");
    Mirror.XRDemoLauncher demoLauncher = world.GetComponent<Mirror.XRDemoLauncher>();
    if (demoLauncher.networkRole == Mirror.XRDemoLauncher.NetworkRoleSelector.Client) return;
    
    roomName = gameObject.name.ToLower();
    MQTTManager.startListening(roomName, ProcessPayload);
  }

  private void ProcessPayload(object payload) {
    RoomPayload roomPayload = payload as RoomPayload;
    if (roomPayload == null || roomPayload.type == null) {
      return;
    }

    Debug.Log(roomPayload);

    string eventId = String.Format("server {0} {1}", roomName, roomPayload.type);
    if (roomPayload.type.Equals("motionSensor")) {
      EventManager.trigger(eventId, "");
    } else if (roomPayload.type.Equals("temperature")) {
      EventManager.trigger(eventId, String.Format("{0}", roomPayload.value));
    } else if (roomPayload.type.Equals("reserved")) {
      EventManager.trigger(eventId, String.Format("{0}", roomPayload.value));
    } else {
      EventManager.trigger(eventId, "");
    }
  }
}
