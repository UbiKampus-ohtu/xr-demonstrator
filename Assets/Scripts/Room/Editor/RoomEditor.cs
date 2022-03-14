using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {

  private void UpdatePositionHandle(Transform transform, ref float targetDimension, Vector3 mask) {
    Vector3 dimension = transform.TransformPoint(targetDimension * mask + new Vector3(0, 1.2f, 0));
    EditorGUI.BeginChangeCheck();
    dimension = Handles.DoPositionHandle(dimension, transform.rotation);
    if (EditorGUI.EndChangeCheck()) {
      Room room = target as Room;
      Undo.RecordObject(room, "Update dimension");
      targetDimension = Vector3.Scale(transform.InverseTransformPoint(dimension), mask).magnitude;
    }
  }

  private void OnSceneGUI() {
    Room room = target as Room;

    Transform handleTransform = room.transform;
    Quaternion handleRotation = handleTransform.rotation;

    Vector3 width = handleTransform.TransformPoint(room.width * Vector3.right);
    Vector3 depth = handleTransform.TransformPoint(room.depth * Vector3.forward);

    UpdatePositionHandle(handleTransform, ref room.width, Vector3.right);
    UpdatePositionHandle(handleTransform, ref room.depth, Vector3.forward);
  }

  private bool DrawWallElementControls(List<WallElement> wallElements) {
    Room room = target as Room;

    for (int index = 0; index < wallElements.Count; index++) {
      EditorGUI.BeginChangeCheck();
      WallElement wallElement = wallElements[index];

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("  Wall Index");
      wallElement.wallIndex = (int)EditorGUILayout.Slider(wallElement.wallIndex, 0, 3);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("  Position");
      float maxDimension = wallElement.wallIndex % 2 != 0 ? room.width : room.depth;
      wallElement.position = EditorGUILayout.Slider(wallElement.position, -maxDimension + wallElement.width * 0.5f, maxDimension - wallElement.width * 0.5f);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("  Width");
      wallElement.width = EditorGUILayout.Slider(wallElement.width, 0.1f, maxDimension * 2f);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("  Height");
      wallElement.height = EditorGUILayout.Slider(wallElement.height, 0.1f, 4);
      EditorGUILayout.EndHorizontal();

      if (EditorGUI.EndChangeCheck()) {
        wallElements[index] = wallElement;
        return true;
      }

      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Copy", GUILayout.Width(85))) {
        wallElements.Add(wallElement);
        return true;
      }
      if (GUILayout.Button("Mirror", GUILayout.Width(85))) {
        wallElement.wallIndex = (int)((wallElement.wallIndex + 2) % 4);
        wallElement.position *= -1;
        wallElements.Add(wallElement);
        return true;
      }
      if (GUILayout.Button("Remove", GUILayout.Width(85))) {
        wallElements.RemoveAt(index);
        return true;
      }
      EditorGUILayout.EndHorizontal();
    }
    return false;
  }

  public override void OnInspectorGUI() {
    Room room = target as Room;

    base.OnInspectorGUI();

    EditorGUILayout.LabelField("Curtains", EditorStyles.boldLabel);
    EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color ( 0.5f,0.5f,0.5f, 1 ) );
    bool updatedCurtains = DrawWallElementControls(room.curtains);

    if (GUILayout.Button("Add curtain")) {
      room.AddWallElement(room.curtains, 2f, 3f);
    }

    EditorGUILayout.LabelField("Doors", EditorStyles.boldLabel);
    EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color ( 0.5f,0.5f,0.5f, 1 ) );
    bool updatedDoors = DrawWallElementControls(room.doors);


    if (GUILayout.Button("Add door")) {
      room.AddWallElement(room.doors, 1.4f, 2.1f);
    }

    if (updatedCurtains || updatedDoors) {
      EditorUtility.SetDirty(room);
    }

    if (GUILayout.Button("Trigger Motionsensor")) {
      room.MotionSensor("");
    }
  }
}
