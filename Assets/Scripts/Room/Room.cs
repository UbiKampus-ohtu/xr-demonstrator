using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallElement {
  public int wallIndex;
  public float position;
  public float width;
  public float height;
  public float activity;
  public GameObject instance;
}

public class Room : MonoBehaviour {
  public string roomName;
  public bool occupied;

  [HideInInspector]
  public float width = 1;
  [HideInInspector]
  public float depth = 1;

  public List<WallElement> curtains;
  public List<WallElement> doors;

  private void Awake() {
    GameObject curtainPrefab = Resources.Load("Prefabs/Furniture/CurtainRoot") as GameObject;
    SpawnWallElements(curtainPrefab, curtains);
    SetWallElementState(occupied, curtains);
    SetWallElementState(occupied, doors);
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
}
