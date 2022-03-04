using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour {
  public Material decalMaterial;
  private int maxQuads = 30000;
  private static DecalManager decalManager;

  private Mesh mesh;
  private void Awake() {
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
    renderer.material = decalMaterial;
    mesh = new Mesh();

    mesh.vertices = new Vector3[4 * maxQuads];
    mesh.uv = new Vector2[4 * maxQuads];
    mesh.triangles = new int[6 * maxQuads];

    meshFilter.mesh = mesh;
  }
}
