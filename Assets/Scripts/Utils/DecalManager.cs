using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour {
  private Dictionary<string, DecalGenerator> generators;

  private static DecalManager decalManager;
  public static DecalManager instance {
    get {
      if (!decalManager) {
        decalManager = FindObjectOfType (typeof(DecalManager)) as DecalManager;
      }

      if (!decalManager) {
      } else {
        decalManager.init();
      }
      return decalManager;
    }

  }

  private void init() {
    if (generators == null) {
      generators = new Dictionary<string, DecalGenerator>();
    }
  }

  public static void addDecal(string name, Vector3 position, Quaternion rotation, float width, float height) {
    DecalGenerator generator = null;
    if (!instance.generators.TryGetValue(name, out generator)) {
      Material material = Resources.Load("Materials/" + name) as Material;
      if (material == null) return;

      GameObject generatorRoot = new GameObject();
      generatorRoot.name = name + " decals";
      generatorRoot.transform.parent = instance.transform;
      generator = generatorRoot.AddComponent<DecalGenerator>();
      generator.SetMaterial(material);
      instance.generators.Add(name, generator);      
    }

    generator.AddDecal(position, rotation, width, height);
  }
}
