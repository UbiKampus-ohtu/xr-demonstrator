using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmission : MonoBehaviour {
	private Color color = new Color(27f, 79f, 191f);
  private Material emissiveMaterial;
  private float emission = 0f;
  private float minEmission = 0.002f;
  private float maxEmission = 0.04f;
  private int steps = 60;
  private float emissionStep;

  private void Awake() {
    emissionStep = (maxEmission - minEmission) / steps;
    emissiveMaterial = GetComponent<MeshRenderer>().material;
  }

  public void SetDuration(float maxDurationInSeconds, float refreshPeriodInSeconds = 1) {
    steps = (int) (maxDurationInSeconds / refreshPeriodInSeconds);
    emissionStep = (maxEmission - minEmission) / steps;
  }

	public void EmissionUpdate(bool state) {
    emission = state ? maxEmission : emission - emissionStep;
    if (emission < minEmission) {
      emission = minEmission;
      return;
    }
    emissiveMaterial.SetColor("_EmissionColor", color * emission);
	}
}