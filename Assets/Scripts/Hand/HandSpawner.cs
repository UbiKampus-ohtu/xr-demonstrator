using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawner : MonoBehaviour {
  public GameObject handPrefab;
  public bool twoHands = false;

  private void spawnHand(string name) {
    GameObject hand = Instantiate(handPrefab);
    hand.transform.parent = this.transform;
    hand.name = name;
  }

  void Start() {
    spawnHand("right hand");
  }
}