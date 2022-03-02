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
    Vector3 footPosition = new Vector3(-0.1f, 0, 0);
    Vector3 footScale = Vector3.one;
    if (isRight) {
      footPosition.x *= -1;
      footScale.x *= -1;
    }
    GameObject foot = Instantiate(footprintPrefab, transform.TransformPoint(footPosition), transform.rotation);
    Destroy(foot, 30);
    foot.transform.localScale = footScale;
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
    } else if (isMoving) {
      SpawnFeet();
      isMoving = false;
    }
  }
}
