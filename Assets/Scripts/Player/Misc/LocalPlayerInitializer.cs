using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LocalPlayerInitializer : NetworkBehaviour {
  public override void OnStartClient() {
    GameObject playerMenu = transform.Find("Menu").gameObject;
    if (!isLocalPlayer) {
      playerMenu.SetActive(false);
      Destroy(playerMenu);
    } else {
      gameObject.AddComponent<PlayerPortal>();
    }
  }
}
