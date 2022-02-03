using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardPen : MonoBehaviour {

  private Vector2 previousPosition = Vector2.zero;
  private bool drawingStarted = false;

  private RaycastHit getNearestSurface() {
    RaycastHit hit;
    LayerMask mask = LayerMask.GetMask("Default");
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.1f, mask)) {
      return hit;
    }
    return new RaycastHit();
  }
  
  private void drawToCanvas(Vector3 worldPosition, GameObject canvas) {
    WhiteBoard whiteboard = canvas.GetComponent<WhiteBoard>();
    Vector3 localPosition = canvas.transform.parent.InverseTransformPoint(worldPosition);
    
    Vector2 currentPosition = new Vector2(localPosition.x, -localPosition.y);

    if (drawingStarted) {
      whiteboard.line(previousPosition.x, currentPosition.x, previousPosition.y, currentPosition.y, Color.black, 3);
    } else {
      whiteboard.draw(currentPosition.x, currentPosition.y, Color.black, 3);
    }
    
    previousPosition = currentPosition;
    drawingStarted = true;
  }

  private void FixedUpdate() {
    RaycastHit nextSurface = getNearestSurface();

    if (nextSurface.collider == null) {
      drawingStarted = false;
      return;
    }

    if (nextSurface.collider.gameObject.name != "Canvas") {
      Debug.Log(nextSurface.collider.gameObject);
      drawingStarted = false;
      return;
    }

    drawToCanvas(nextSurface.point, nextSurface.collider.gameObject);
  }
}
