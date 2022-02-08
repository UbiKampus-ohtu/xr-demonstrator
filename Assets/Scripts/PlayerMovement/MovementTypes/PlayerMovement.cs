using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  [HideInInspector]
  public Movement movement;
  public PlayerSettings settings; 

  public virtual void Start() {
    movement = GetComponent<Movement>();
    if (movement == null) {
      movement = gameObject.AddComponent<Movement>();
    }
    movement.settings = settings;
  }
}