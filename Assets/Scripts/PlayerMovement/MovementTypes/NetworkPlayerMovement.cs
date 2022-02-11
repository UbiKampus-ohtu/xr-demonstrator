using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerMovement : PlayerMovement {
  private Vector3 playerPosition = Vector3.zero;
  private Vector3 playerRotation = Vector3.zero;

  public override void Start() {
    base.Start();
    movement.gravity = false;
    NetworkManager.startListening("player id_here", doSomething);
  }

  private void doSomething(string payload) {
    MovementPayload movementPayload = PlayerSerializations.JsonStringToSerialization(payload);
    playerPosition = movementPayload.pos;
  }

  private void Update() {
    movement.setPosition(playerPosition);
  }

}
