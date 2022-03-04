using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintSpawner : MonoBehaviour {
  public GameObject footprintPrefab;

  private bool isMoving = false;
  private bool isRight = false;
  private float distance = 0f;
  private Vector3 previousPosition = Vector3.zero;

  private void SpawnFoot() {
    Vector3 footPosition = new Vector3(-0.2f, 0, 0);
    Vector3 footScale = Vector3.one;
    float footWidth = 0.3f;
    float footHeight = footWidth;
    if (isRight) {
      footWidth *= -1;
      footPosition.x *= -1;
      footScale.x *= -1;
    }
    Vector3 decalPosition = transform.TransformPoint(footPosition);
    DecalManager.addDecal("Footprint", decalPosition, transform.rotation, footWidth, footHeight);

    isRight = !isRight;
    distance = 0f;
  }

  private void SpawnFeet() {
    SpawnFoot();
    SpawnFoot();
  }

  private void Update() {
    if (previousPosition != transform.position) {
      isMoving = true;
      distance += (transform.position - previousPosition).magnitude;
      previousPosition = transform.position;
      if (distance > 0.5f) {
        SpawnFoot();
      }
    } else if (isMoving && distance > 0.15f) {
      SpawnFeet();
      isMoving = false;
    }
  }
}
