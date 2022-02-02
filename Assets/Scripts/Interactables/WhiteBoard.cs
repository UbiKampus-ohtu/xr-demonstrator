using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoard : MonoBehaviour {
  public Color whiteboardBackground;
  //public RenderTexture whiteboardTexture;
  private Texture2D canvas;
  
  private int height = 128;
  private int width = 128;

  private Color[] pixels;

  private void Start() {
    canvas = new Texture2D(width, height);
    MeshRenderer whiteboardMeshRenderer = GetComponent<MeshRenderer>();
    whiteboardMeshRenderer.material.mainTexture = canvas;
    canvas.filterMode = FilterMode.Point;
    pixels = canvas.GetPixels(0);

    fillRect(0, 1, 0, 1, Color.green);
  }

  public void draw(float x0, float y0, Color color) {
    int x = (int)(x0 % 1 * width);
    int y = (int)(y0 % 1 * height);

    pixels[x + width * y] = color;
    apply();
  }

  public void fillRect (float x0, float x1, float y0, float y1, Color color) {
    int xStart = (int)(x0 * width);
    int xEnd = (int)(x1 * width);
    int yStart = (int)(y0 * height);
    int yEnd = (int)(y1 * height);

    for (int y = yStart; y < yEnd; y++) {
      for (int x = xStart; x < xEnd; x++) {
        pixels[x + width * y] = color;
      }
    }

    apply();
  }

  private void apply() {
    canvas.SetPixels(pixels, 0);
    canvas.Apply();
  }

  /*
  float j = 0;
  private void Update() {
    j += 0.001f;
    rect(0, j, 0, 1, Color.red);
  }
  */
}
