using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementSpawner : MonoBehaviour {
  public enum PlayerEnum{local, network, npc};

  public PlayerEnum playerType;
  public PlayerSettings settings;

  private void Start() {
    PlayerMovement movement;
    switch(playerType) {
      case PlayerEnum.local:
        movement = gameObject.AddComponent<InputPlayerMovement>();
        movement.settings = settings;
        break;
      
      case PlayerEnum.network:
        movement = gameObject.AddComponent<NetworkPlayerMovement>();
        movement.settings = settings;
        break;

      case PlayerEnum.npc:
        Debug.Log("not implemented");
        break;
    }
  }
}
