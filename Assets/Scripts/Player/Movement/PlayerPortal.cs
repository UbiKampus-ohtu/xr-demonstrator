using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortal : MonoBehaviour {
  private Portal portalToBeEnabled = null;
  private bool warp = false;
  private Vector3 warpPosition = Vector3.zero;

  private void OnControllerColliderHit(ControllerColliderHit hit) {
    if (hit.collider.name.Contains("Portal")) {
      MoveThroughPortal(hit.collider.gameObject);
    } else if (portalToBeEnabled != null && hit.collider.name.Contains("Floor")) {
      portalToBeEnabled.isEnabled = true;
      portalToBeEnabled = null;
    }
  }

  private void MoveThroughPortal(GameObject portalRoot) {
    Portal portal = portalRoot.GetComponent<Portal>();
    if (portal == null || portal.target == null || !portal.isEnabled) return;

    portal.targetPortal.isEnabled = false;
    
    portalToBeEnabled = portal.targetPortal;
    warpPosition = portal.target.transform.position;
    warp = true;
  }

  private void LateUpdate() {
    if (!warp) return;
    transform.position = warpPosition;
    warp = false;
  }
}
