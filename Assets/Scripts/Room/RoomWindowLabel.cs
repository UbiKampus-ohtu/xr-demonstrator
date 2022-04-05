using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWindowLabel : MonoBehaviour {
  private void Awake() {
    for (int i = 0; i < transform.childCount; i++) {
      Transform child = transform.GetChild(i);
      if (child.CompareTag("Label")) {
        TextMesh windowLabelTM = child.GetComponent<TextMesh>();
        windowLabelTM.text = gameObject.name;
      }
    }
  }
}
