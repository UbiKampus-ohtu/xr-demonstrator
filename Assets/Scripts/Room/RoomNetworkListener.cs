using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomPayload {
  public string type;
  public int value;
}

public class RoomNetworkListener : MonoBehaviour {
  private string roomName;

  private void OnEnable() {
    roomName = gameObject.name;
    MQTTManager.startListening(roomName, ProcessPayload);
  }

  private void ProcessPayload(object payload) {
    RoomPayload roomPayload = JsonUtility.FromJson<RoomPayload>((string) payload);
    if (roomPayload == null || roomPayload.type == null) {
      return;
    }

    string eventId = String.Format("{0} {1}", roomName, roomPayload.type);

    if (roomPayload.type.Equals("motionSensor")) {
      EventManager.trigger(eventId, "");
    } else if (roomPayload.type.Equals("temperature")) {
      EventManager.trigger(eventId, String.Format("{0}", roomPayload.value));
    } else {
      EventManager.trigger(eventId, "");
    }
  }
}
