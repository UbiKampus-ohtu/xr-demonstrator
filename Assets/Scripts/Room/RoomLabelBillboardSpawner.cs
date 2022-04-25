using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLabelBillboardSpawner : MonoBehaviour {
  private void Awake() {
    GameObject labelPrefab = Resources.Load("Prefabs/RoomLabelBillboard") as GameObject;
    GameObject roomLabel = Instantiate(labelPrefab, transform.position + new Vector3(0, 2.4f, 0), Quaternion.Euler(0, 180, 0));
    TextMesh labelTextMesh = roomLabel.GetComponent<TextMesh>();
    labelTextMesh.text = gameObject.name;
    roomLabel.transform.parent = transform;
  }
}
