using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallElement {
  public int wallIndex;
  public float position;
  public float width;
  public float height;
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
