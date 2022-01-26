using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHand : MonoBehaviour {
  public GameObject handPrefab;
  public bool twoHands = false;

  private void spawnHand() {
    GameObject hand = Instantiate(handPrefab);
    hand.transform.parent = this.transform;
  }

  void Start() {
    spawnHand();
  }
}
