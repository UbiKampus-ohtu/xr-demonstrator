using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentDisplay : MonoBehaviour {
  public int digits = 4;
  public GameObject segmentPrefab;
  private float segmentWidth = 0.06f;
  private List<SegmentDisplaySegment> segments = new List<SegmentDisplaySegment>();

  private void Start() {
    for (int i = 0; i < digits; i++) {
      GameObject segment = Instantiate(segmentPrefab);
      segment.transform.parent = transform;
      segment.transform.localPosition = new Vector3(i * segmentWidth, 0, 0);
      segment.transform.localScale = Vector3.one;
      segment.transform.localRotation = Quaternion.identity;
      segments.Add(segment.GetComponent<SegmentDisplaySegment>());
      segments[i].init();
    }
    clear();
  }

  public void clear() {
    set("0000000000000000");
  }

  public void set(string message) {
    int j = message.Length - 1;
    for (int i = 0; i < digits && j >= 0; i++) {
      segments[i].set(message[j]);
      j--;
    }
  }
}
