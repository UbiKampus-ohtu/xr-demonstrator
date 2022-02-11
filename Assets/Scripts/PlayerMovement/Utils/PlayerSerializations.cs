using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementPayload {
  public Vector3 pos;
  public Vector3 rot;
}

class PlayerSerializations {

  public static MovementPayload JsonStringToSerialization(string payload) {
    MovementPayload movementPayload = JsonUtility.FromJson<MovementPayload>(payload);
    return movementPayload;
  }

  public static string PlayerTransformToJsonString(Vector3 position, Quaternion rotation) {
    MovementPayload movementPayload = new MovementPayload();
    movementPayload.pos = position;
    movementPayload.rot = rotation.eulerAngles;
    return JsonUtility.ToJson(movementPayload);
  }

}