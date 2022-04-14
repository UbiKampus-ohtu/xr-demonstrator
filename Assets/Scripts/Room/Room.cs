using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public struct WallElement {
  public int wallIndex;
  public float position;
  public float width;
  public float height;
  public GameObject instance;
}

public class Room : NetworkBehaviour {
  [SyncVar(hook = nameof(OccupiedSyncHook))]
  public bool occupied;

  [SyncVar(hook = nameof(MotionSyncHook))]
  private int motionCounter = 0;

  [HideInInspector]
  public float width = 1;
  [HideInInspector]
  public float depth = 1;

  [HideInInspector]
  public List<WallElement> curtains;
  [HideInInspector]
  public List<WallElement> doors;

  public string roomName;

  private void OccupiedSyncHook(bool oldState, bool newState) {
    occupied = newState;
    occupiedStateChanged = true;
    EventManager.trigger(string.Format("{0} reserved", roomName), occupied ? "1" : "0");
  }
  
  private float feetPeriod = 10;
  private void MotionSyncHook(int oldValue, int newValue) {
    if (newValue <= oldValue) return;

    motionCounter = newValue;

    if (!hasActivity) {
      feetPeriod = 1;
    }

    float feetPerSecond = 1 / feetPeriod;
    float feetPerMin = feetPerSecond * 60;
    SetFeetPerMin(feetPerMin + 60);

    EventManager.trigger(string.Format("{0} motionSensor", roomName), "");
  }

  private void Awake() {
    GameObject doorPrefab = Resources.Load("Prefabs/Furniture/Door") as GameObject;
    GameObject curtainPrefab = Resources.Load("Prefabs/Furniture/CurtainRoot") as GameObject;
    SpawnWallElements(curtainPrefab, curtains);
    SpawnWallElements(doorPrefab, doors);
    SetWallElementState(occupied, curtains);
    SetWallElementState(occupied, doors);
    SpawnTriggerVolume();
    roomName = gameObject.name;
    gameObject.AddComponent<RoomLabelBillboardSpawner>();
    gameObject.AddComponent<RoomMotionSensorSpawner>();
    gameObject.AddComponent<RoomWindowLabel>();
  }

  public override void OnStartServer() {
    base.OnStartServer();
    gameObject.AddComponent<RoomNetworkListener>();
    SetWallElementState(occupied, curtains);
    SetWallElementState(occupied, doors);
    EventManager.startListening(string.Format("server {0} reserved", gameObject.name), Reservation);
    EventManager.startListening(string.Format("server {0} motionSensor", gameObject.name), MotionSensor);
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
        wallElement.instance.SetActive(state);
        continue;
      }
      wallElementAnimator.SetBool("closed", state);
    }
  }

  private void SpawnWallElements(GameObject wallElementPrefab, List<WallElement> wallElements) {
    if (wallElements.Count == 0 || wallElementPrefab == null) return;

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
  
  public void SetFeetPerMin(float fpm) {
    if (fpm < 1) {
      hasActivity = false;
      return;
    }
    hasActivity = true;
    float feetPerSecond = fpm / 60;
    feetPeriod = 1 / feetPerSecond;
  }

  public void MotionSensor(string param) {
    MotionSyncHook(motionCounter, motionCounter + 1);
  }

  private bool occupiedStateChanged = true;
  public void Reservation(string param) {
    bool state = param == "1" ? true : false;
    if (occupied == state) return;
    OccupiedSyncHook(occupied, state);
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
    DecalManager.addDecal("FootprintSim", transform.position + deltaPosition, rotation, 0.25f, 0.25f);
  }
  
  private void Update() {
    SpawnFootprint();
    if (occupiedStateChanged) {
      occupiedStateChanged = false;
      SetWallElementState(occupied, curtains);
      SetWallElementState(occupied, doors);
    }
  }
}
