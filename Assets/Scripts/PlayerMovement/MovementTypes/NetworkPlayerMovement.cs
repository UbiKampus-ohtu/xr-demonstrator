using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementPayload {
  public Vector3 pos;
  public Vector3 dir;
}

public class NetworkPlayerMovement : PlayerMovement {
  private Vector3 playerPosition = Vector3.zero;

  public override void Start() {
    base.Start();
    movement.gravity = false;
    NetworkManager.startListening("player id_here", doSomething);
  }

  private void doSomething(string payload) {
    MovementPayload movementPayload = JsonUtility.FromJson<MovementPayload>(payload);
    playerPosition = movementPayload.pos;
  }

  private void Update() {
    movement.setPosition(playerPosition);
  }

}
