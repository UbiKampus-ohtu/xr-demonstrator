using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalGenerator : MonoBehaviour {
  private int decals = 0;
  private int maxDecals = 100;
  private bool firstPass = true;

  private Mesh mesh;
  private Vector3 [] vertices;
  private Vector2 [] uv;
  private int [] triangles;

  private void Awake() {
    MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    mesh = new Mesh();

    vertices = new Vector3[4 * maxDecals];
    uv = new Vector2[4 * maxDecals];
    triangles = new int[6 * maxDecals];

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;
    mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
    meshFilter.mesh = mesh;
  }

  public void SetMaterial(Material material) {
    MeshRenderer renderer = GetComponent<MeshRenderer>();
    if (renderer == null) return;
    renderer.material = material;
  }

  public void AddDecal(Vector3 position, Quaternion rotation, float width = 1, float height = 1) {
    if (decals >= maxDecals) {
      decals = 0;
      firstPass = false;
    }

    int uvx0 = 0;
    int uvx1 = 1;
    int uvy0 = 0;
    int uvy1 = 1;

    if (width < 0) {
      width = -width;
      uvx0 = 1;
      uvx1 = 0;
    }

    if (height < 0) {
      height = -height;
      uvy0 = 1;
      uvy1 = 0;
    }

    height = height < 0 ? -height : height;

    Vector3[] normals = mesh.normals;

    Vector3 [] decalVertices = {
      new Vector3(-width, 0, -height),
      new Vector3(-width, 0, height),
      new Vector3(width, 0, height),
      new Vector3(width, 0, -height)
    };

    int cursor = decals * 4;
    int triangleCursor = decals * 6;

    for (int i = 0; i < 4; i++) {
      vertices[cursor + i] = position + rotation * decalVertices[i];
      uv[i + cursor] = new Vector2(i > 1 ? uvx1 : uvx0, i > 0 && i < 3 ? uvy1 : uvy0);
    }

    if (firstPass) {
      int [] edges = {0, 1, 2, 0, 2, 3};
      for (int i = 0; i < 6; i++) {
        triangles[i + triangleCursor] = cursor + edges[i];
      }
      mesh.triangles = triangles;
      mesh.uv = uv;
    }

    mesh.vertices = vertices;
    decals++;
  }
}
