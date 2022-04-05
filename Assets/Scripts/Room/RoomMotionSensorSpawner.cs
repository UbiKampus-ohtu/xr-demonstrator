using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMotionSensorSpawner : MonoBehaviour {
  private void Awake() {
    GameObject motionSensorPrefab = Resources.Load("Prefabs/MotionSensor") as GameObject;
    GameObject motionSensor = Instantiate(motionSensorPrefab, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
    motionSensor.transform.parent = transform;
  }
}
