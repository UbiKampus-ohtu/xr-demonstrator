using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallElement {
  public int wallIndex;
  public float position;
  public float width;
  public float height;
  public GameObject instance;
}

public class Room : MonoBehaviour {
  public bool occupied;

  [HideInInspector]
  public float width = 1;
  [HideInInspector]
  public float depth = 1;

  [HideInInspector]
  public List<WallElement> curtains;
  [HideInInspector]
  public List<WallElement> doors;

  private void Awake() {
    GameObject curtainPrefab = Resources.Load("Prefabs/Furniture/CurtainRoot") as GameObject;
    SpawnWallElements(curtainPrefab, curtains);
    SetWallElementState(occupied, curtains);
    SetWallElementState(occupied, doors);
    SpawnTriggerVolume();
  }

  private void SpawnTriggerVolume() {
    BoxCollider triggerVolume = gameObject.AddComponent<BoxCollider>();
    triggerVolume.center = new Vector3(0, 1.5f, 0);
    triggerVolume.size = new Vector3(width * 2, 3f, depth * 2);
    triggerVolume.isTrigger = true;
  }

  private void SetWallElementState(bool state, List<WallElement> wallElements) {
    foreach(WallElement wallElement in wallElements) {
      Animator wallElementAnimator = wallElement.instance.GetComponentInChildren<Animator>();
      if (wallElementAnimator == null) {
        continue;
      }
      wallElementAnimator.SetBool("closed", occupied);
    }
  }

  private void SpawnWallElements(GameObject wallElementPrefab, List<WallElement> wallElements) {
    for (int i = 0; i < wallElements.Count; i++) {
      WallElement wallElement = wallElements[i];
      GameObject thisWallElement = Instantiate(wallElementPrefab);
      thisWallElement.transform.parent = this.transform;

      Quaternion rotation = Quaternion.Euler(0, 90f * wallElement.wallIndex + 90f, 0);
      thisWallElement.transform.localRotation = rotation;

      float distance = wallElement.wallIndex % 2 == 0 ? width : depth;
      thisWallElement.transform.localPosition = rotation * new Vector3(-wallElement.position, 0, distance);
      thisWallElement.transform.localScale = new Vector3(wallElement.width, wallElement.height, 1);

      wallElement.instance = thisWallElement;
      wallElements[i] = wallElement;
    }
  }

  private void DrawWallElements(List<WallElement> wallElements, Color color) {
    Gizmos.color = color;
    foreach (WallElement wallElement in wallElements) {
      Quaternion rotation = Quaternion.Euler(0, wallElement.wallIndex * 90f, 0);
      if (wallElement.wallIndex % 2 == 0) {
        Gizmos.DrawCube(rotation * new Vector3(width, wallElement.height * 0.5f, wallElement.position), rotation * new Vector3(0.1f, wallElement.height, wallElement.width));
      } else {
        Gizmos.DrawCube(rotation * new Vector3(depth, wallElement.height * 0.5f, wallElement.position), new Vector3(wallElement.width, wallElement.height, 0.1f));
      }
    }
  }

  public void AddWallElement(List<WallElement> wallElements, float width, float height) {
    WallElement wallElement = new WallElement();
    wallElement.wallIndex = 0;
    wallElement.width = width;
    wallElement.position = 0;
    wallElement.height = height;
    wallElements.Add(wallElement);
  }

  private void OnDrawGizmos() {
    Color roomGizmoColor = Color.red;
    roomGizmoColor.a = 0.5f;
    Gizmos.matrix = transform.localToWorldMatrix;
    Gizmos.color = roomGizmoColor;
    Gizmos.DrawCube(new Vector3(0, 1.5f, 0), new Vector3(2 * width, 3f, 2 * depth));

    DrawWallElements(curtains, new Color(0, 0, 1, 0.5f));
    DrawWallElements(doors, new Color(1, 1, 1, 0.5f));
  }

  private bool hasActivity = false;
  private float feetTimer = 0;
  private float feetPeriod = 10;
  public void SetFeetPerMin(float fpm) {
    if (fpm < 1) {
      hasActivity = false;
      return;
    }
    hasActivity = true;
    float feetPerSecond = fpm / 60;
    feetPeriod = 1 / feetPerSecond;
  }

  public void MotionSensor() {
    if (!hasActivity) {
      feetPeriod = 1;
    }
    float feetPerSecond = 1 / feetPeriod;
    float feetPerMin = feetPerSecond * 60;
    SetFeetPerMin(feetPerMin + 60);
  }

  private void SpawnFootprint() {
    if (!hasActivity) return;

    feetTimer += Time.deltaTime;
    feetPeriod += Time.deltaTime * 0.1f;
    if (feetTimer < feetPeriod) return;

    if (feetPeriod > 60) {
      hasActivity =  false;
    }

    feetTimer = 0;
    Vector3 deltaPosition = transform.rotation * new Vector3(Random.Range(-width + 0.3f, width - 0.3f), 0, Random.Range(-depth + 0.3f, depth - 0.3f));
    Quaternion rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
    DecalManager.addDecal("FootprintSim", transform.position + deltaPosition, rotation, 0.3f, 0.3f);
  }
  
  private void Update() {
    SpawnFootprint();
  }
}
