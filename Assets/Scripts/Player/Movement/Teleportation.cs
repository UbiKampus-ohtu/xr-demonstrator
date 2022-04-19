using UnityEngine;

public class Teleportation : MonoBehaviour {
  private Camera _camera;
  private GameObject playerCharacter;
  private CharacterController controller;
  public GameObject teleportationSphere;
  private GameObject sphere;

  private MovementActions mv;
  private Vector3 deltaPosition = Vector3.zero;

  private bool initialized = false;

  private TeleporterHandle teleporterHandle;

  void Awake() {
    mv = new MovementActions();
  }

  void OnEnable() {
    mv.Enable();
  }

  private bool debounced = true;
  void LateUpdate() {
    bool keyPressed = mv.Player.teleport.ReadValue<Vector2>().y >= 0.3f;
    if (debounced && keyPressed) {
      debounced = false;
      SpawnTeleportationCircle();
    } else if (!debounced && !keyPressed) {
      debounced = true;
      HandleTeleportation();
    }

    if (keyPressed) {
      UpdateTeleportationCircle(transform.position, transform.TransformDirection(Vector3.forward));
    } else {
      return;
    }

    if (mv.Player.click.ReadValue<float>() > 0) {
      HandleTeleportation();
    }
  }

  private void SpawnTeleportationCircle() {
    sphere = Instantiate(teleportationSphere);
    teleporterHandle = sphere.GetComponent<TeleporterHandle>();
  }

  private void UpdateTeleportationCircle(Vector3 rayOrigin, Vector3 rayDirection, int depth = 0) {
    if (sphere == null) return;

    LayerMask targetMask = LayerMask.GetMask("Default");
  
    float distance = 10f;

    Ray ray = new Ray(rayOrigin, rayDirection);
    if (Physics.Raycast(ray, out var hit, distance, targetMask)) {
      if (hit.transform.CompareTag("Floor")) {
        deltaPosition = hit.point - transform.position + new Vector3(0, 0.05f, 0);
        sphere.transform.position = hit.point;
        teleporterHandle.setValid(true);
      } else {
        if (depth < 1) {
          float hitDistance = (hit.point - transform.position).magnitude;
          Vector3 backtrack = (hit.point - transform.position).normalized;
          Vector3 newRayOrigin = hit.point - backtrack;
          UpdateTeleportationCircle(newRayOrigin, -Vector3.up, depth + 1);
        } else {
          deltaPosition = Vector3.zero;
          sphere.transform.position = hit.point;
          teleporterHandle.setValid(false);
        }
      }
    } else if (depth < 1) {
      Vector3 newRayOrigin = transform.TransformPoint(Vector3.forward * distance);
      UpdateTeleportationCircle(newRayOrigin, -Vector3.up, depth + 1);
    } else {
      deltaPosition = Vector3.zero;
      teleporterHandle.setValid(false);
    }
  }

  private void HandleTeleportation() {
    Init();
    if (controller != null) {
      controller.Move(deltaPosition);
      deltaPosition = Vector3.zero;
    }
    CollectTeleportationCircle();
  }

  private void CollectTeleportationCircle() {
    if (teleporterHandle != null) {
      teleporterHandle.exit();
      teleporterHandle = null;
    }
    Destroy(sphere);
    deltaPosition = Vector3.zero;
  }

  private void Init() {
    if (initialized) return;

    GameObject cursor = gameObject;
    while (controller == null) {
      controller = cursor.GetComponent<CharacterController>();
      if (cursor.transform.parent == null) {
        break;
      } else {
        cursor = cursor.transform.parent.gameObject;
      }
    }

    if (controller == null) {
      Destroy(gameObject);
    }
    initialized = true;
  }
}