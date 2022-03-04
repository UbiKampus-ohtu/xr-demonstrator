using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour {
  public Material decalMaterial;
  private int decals = 0;
  private int maxDecals = 30;
  private static DecalManager decalManager;

  private Mesh mesh;
  private Vector3 [] vertices;
  private Vector2 [] uv;
  private int [] triangles;

  private void Awake() {
    MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    renderer.material = decalMaterial;
    mesh = new Mesh();

    vertices = new Vector3[4 * maxDecals];
    uv = new Vector2[4 * maxDecals];
    triangles = new int[6 * maxDecals];

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;

    meshFilter.mesh = mesh;

    EventManager.startListening("nopeustesti pressed", asd);
  }

  public void asd(string param) {
    AddDecal(new Vector3(decals, decals), Quaternion.identity);
  }

  public void AddDecal(Vector3 position, Quaternion rotation, float width = 1, float height = 1) {
    if (decals >= maxDecals) decals = 0;

    Vector3 [] decalVertices = {
      new Vector3(-width, 0, -height),
      new Vector3(-width, 0, height),
      new Vector3(width, 0, height),
      new Vector3(width, 0, -height)
    };

    int cursor = decals * 4;

    for (int i = 0; i < 4; i++) {
      vertices[i + cursor] = rotation * (position + decalVertices[i]);
      uv[i + cursor] = new Vector2(i > 1 ? 1 : 0, i > 0 && i < 3 ? 1 : 0);
    }
    int triangleCursor = decals * 6;
    int [] edges = {0, 1, 2, 0, 2, 3};

    for (int i = 0; i < 6; i++) {
      triangles[i + triangleCursor] = decals + edges[i];
    }

    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uv;

    Debug.Log("added decal");
    decals++;
  }
}
