using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
  [HideInInspector]
  public PlayerSettings settings;
  [HideInInspector]
  public bool xrEnabled = false;
  [HideInInspector]
  public bool gravity = true;

  private Translate playerTranslation;
  private Turn playerTurn;
  [HideInInspector]
  public Tilt playerTilt;

  private Transform offset;
  private Transform viewport;

  public bool initialized = false;

  private void Start() {
    playerTranslation = gameObject.AddComponent<Translate>();
    playerTurn = gameObject.AddComponent<Turn>();
    playerTilt = gameObject.AddComponent<Tilt>();

    offset = transform.Find("Offset");
    viewport = offset.Find("Viewport");

    playerTilt.setTarget(offset);
    playerTranslation.colliderRadius = settings.colliderRadius;
    playerTranslation.gravityEnabled = gravity;

    if (!xrEnabled) {
      offset.transform.position += new Vector3(0f, settings.playerEyelineHeight, 0f);
    }

    initialized = true;
  }

  public void move(Vector2 movementInput) {
    Quaternion referenceRotation = viewport.rotation * Quaternion.Euler(-viewport.rotation.eulerAngles.x, 0, -viewport.rotation.eulerAngles.z);
    playerTranslation.move(movementInput * settings.walkingSpeed, referenceRotation);
  }

  public void offsetPosition(Vector3 deltaPosition) {
    playerTranslation.setPosition(deltaPosition);
  }

  public void setPosition(Vector3 position) {
    playerTranslation.setAbsolutePosition(position);
  }

  public void turn(float angle) {
    playerTurn.turn(angle * settings.mouseSensitivity);
  }

  public void tilt(float angle) {
    if (xrEnabled) {
      return;
    }
    playerTilt.tilt(angle * settings.mouseSensitivity);
  }

  public void setViewportPosition(Vector3 localPosition) {
    viewport.localPosition = localPosition;
  }

  public void setViewportRotation(Quaternion localRotation) {
    viewport.localRotation = localRotation;
  }
}
