using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour {
  public bool active = false;
  public string logs = "";
  public int lines = 0;

  private static ConsoleToGUI consoleToGUI;

  public static ConsoleToGUI instance {
    get {
      if (!consoleToGUI) {
        consoleToGUI = FindObjectOfType(typeof(ConsoleToGUI)) as ConsoleToGUI;
      }

      return consoleToGUI;
    }
  }  private void init() {
    logs = "logging started";
  }

  public static void Log(string log) {
    if (instance.logs.Length + log.Length > 1000) {
      instance.logs = "";
    }
    instance.logs += "\n" + log;
  }

  private Vector3 GetScreenScale() {
    return new Vector3(Screen.width / 1200f, Screen.height / 800f, 1f);
  }

  private void OnGUI() {
    if (!active) return;

    GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, GetScreenScale());
    GUI.TextArea(new Rect(10, 10, 540, 370), instance.logs);
  }
}
