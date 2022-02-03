using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoard : MonoBehaviour {
  public Color whiteboardBackground;
  private Texture2D canvas;
  
  private int height = 1024;
  private int width = 1024;

  private Color[] pixels;

  private void Start() {
    canvas = new Texture2D(width, height);
    MeshRenderer whiteboardMeshRenderer = GetComponent<MeshRenderer>();
    whiteboardMeshRenderer.material.mainTexture = canvas;
    canvas.filterMode = FilterMode.Point;
    pixels = canvas.GetPixels(0);

    fillRect(0, 1, 0, 1, whiteboardBackground);
  }

  public void draw(float x0, float y0, Color color, int radius = 0) {
    int x = (int)(x0 % 1 * width);
    int y = (int)(y0 % 1 * height);

    drawInt(x, y, color, radius);
    apply();
  }

  private void drawInt(int x, int y, Color color, int radius = 0) {
    int cursor = x + width * y;
    if (radius == 0) {
      if (cursor >= 0 && cursor < width * height) {
        pixels[x + width * y] = color;
      }
      return;
    }

    fillRectInt(x - radius, x + radius, y - radius, y + radius, color);
  }

  public void line(float _x0, float _x1, float _y0, float _y1, Color color, int radius = 0) {
    int x0 = (int)(_x0 % 1 * width);
    int x1 = (int)(_x1 % 1 * width);
    int y0 = (int)(_y0 % 1 * height);
    int y1 = (int)(_y1 % 1 * height);

    int xStep = x1 - x0;
    int yStep = y1 - y0;

    float distance = new Vector2(xStep, yStep).magnitude;
    int steps = (int)distance;
    if (radius > 0) {
      steps = (int)(distance / (float)radius);
    }

    for (int i = 1; i < steps; i++) {
      float coeff = (float)i / steps;
      int dx = (int)(xStep * coeff);
      int dy = (int)(yStep * coeff);
      drawInt(x0 + dx, y0 + dy, color, radius);
    }
    drawInt(x1, y1, color, radius);
    apply();
  }

  private void fillRectInt (int x0, int x1, int y0, int y1, Color color) {
    for (int y = y0; y < y1; y++) {
      for (int x = x0; x < x1; x++) {
        int cursor = x + width * y;
        if (cursor >= 0 && cursor < width * height) {
          pixels[x + width * y] = color;
        }
      }
    }
  }

  public void fillRect (float x0, float x1, float y0, float y1, Color color) {
    int xStart = (int)(x0 * width);
    int xEnd = (int)(x1 * width);
    int yStart = (int)(y0 * height);
    int yEnd = (int)(y1 * height);

    fillRectInt(xStart, xEnd, yStart, yEnd, color);
    apply();
  }

  private void apply() {
    canvas.SetPixels(pixels, 0);
    canvas.Apply();
  }
}
