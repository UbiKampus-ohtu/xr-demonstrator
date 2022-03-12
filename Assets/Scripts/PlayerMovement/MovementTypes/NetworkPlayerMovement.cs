using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerMovement : PlayerMovement {
  private Vector3 playerPosition = Vector3.zero;
  private Quaternion playerRotation = Quaternion.identity;

  public override void Start() {
    base.Start();
    movement.gravity = false;
    MQTTManager.startListening("player id_here 0 transform", updateTransform);
    //NetworkManager.startListening("player id_here 1 transform", updateRightHand);
    //NetworkManager.startListening("player id_here 2 transform", updateHead);
    //NetworkManager.startListening("player id_here 3 transform", updateLeftHand);
  }

  private void updateTransform(object payload) {
    float [] transform = (float [])payload;
    playerPosition = new Vector3(transform[0], transform[1], transform[2]);
    playerRotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);
  }

  private void Update() {
    movement.setPosition(playerPosition);
  }

}
