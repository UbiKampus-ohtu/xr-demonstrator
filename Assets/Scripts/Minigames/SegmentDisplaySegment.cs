using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentDisplaySegment : MonoBehaviour {
  private bool initialized = false;
  private Dictionary<string, GameObject> segments = new Dictionary<string, GameObject>();
  private Dictionary<char, List<string>> letters = new Dictionary<char, List<string>> {
      {'0', new List<string> {"BL", "BR", "LL", "LR", "UL", "UR", "TL", "TR"}},
      {'1', new List<string> {"LR", "UR", "TR"}},
      {'2', new List<string> {"BL", "BR", "LL", "CML", "CMR", "UR", "TR", "TL"}},
      {'3', new List<string> {"BL", "BR", "LR", "UR", "TR", "TL", "CMR", "CML"}},
      {'4', new List<string> {"UL", "CML", "CMR", "UR", "LR"}},
      {'5', new List<string> {"BL", "BR", "LR", "CMR", "CML", "UL", "TR", "TL"}},
      {'6', new List<string> {"BL", "BR", "LR", "CMR", "CML", "UL", "TR", "TL", "LL"}},
      {'7', new List<string> {"TL", "TR", "CUR", "CLL", "CML"}},
      {'8', new List<string> {"BL", "BR", "LL", "LR", "UL", "UR", "TL", "TR", "CMR", "CML"}},
      {'9', new List<string> {"BL", "BR", "LR", "UL", "UR", "TL", "TR", "CMR", "CML"}}
  };

  private void Start() {
    if (!initialized) {
      init();
    }
  }

  public void init() {
    Dictionary<string, string> tags = new Dictionary<string, string> {
      {"BL", "segmentBottomLeft"},
      {"BR", "segmentBottomRight"},
      {"CLL", "segmentCrossLowerLeft"},
      {"CLM", "segmentCrossLowerMiddle"},
      {"CLR", "segmentCrossLowerRight"},
      {"CML", "segmentCrossMiddleLeft"},
      {"CMR", "segmentCrossMiddleRight"},
      {"CUL", "segmentCrossUpperLeft"},
      {"CUM", "segmentCrossUpperMiddle"},
      {"CUR", "segmentCrossUpperRight"},
      {"LL", "segmentLowerLeft"},
      {"LR", "segmentLowerRight"},
      {"TL", "segmentTopLeft"},
      {"TR", "segmentTopRight"},
      {"UL", "segmentUpperLeft"},
      {"UR", "segmentUpperRight"},
      {"dot", "dot"}
    };
    foreach (string key in tags.Keys) {
      GameObject segment = transform.Find(tags[key]).gameObject;
      segments.Add(key, segment);
    }
    initialized = true;
  }

  public void clear() {
    foreach (string key in segments.Keys) {
      segments[key].SetActive(false);
    }
  }

  public void set(char letter) {
    clear();
    List<string> keys = letters[letter];
    foreach(string key in keys) {
      segments[key].SetActive(true);
    }
  }
}
