using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject {
  public string prefabName;

  [Header("Movement settings")]
  [Range(1.1f, 2.05f)]
  [Tooltip("m/s")]
  public float walkingSpeed = 1.1f;

  [Range(2.05f, 3.58f)]
  [Tooltip("m/s")]
  public float runSpeed = 2.05f;

  public float mouseSensitivity = 0.5f;

  public float playerEyelineHeight = 1.65f;

  public float colliderRadius = 0.25f;
}