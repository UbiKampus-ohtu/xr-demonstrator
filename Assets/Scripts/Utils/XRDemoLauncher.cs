using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {

  [DisallowMultipleComponent]
  [AddComponentMenu("Network/XR Demonstrator launcher")]
  [RequireComponent(typeof(NetworkManager))]
  
  public class XRDemoLauncher : MonoBehaviour {
    
    public enum NetworkRoleSelector {
      Client = 0,
      Host = 1,
      ListeningServer = 2
    }

    public NetworkRoleSelector networkRole;

    private NetworkManager manager;

    private void Awake() {
      manager = GetComponent<NetworkManager>();

      StartCoroutine(init());
    }

    IEnumerator init() {
      // wait for scene to initialize
      yield return CoroutineUtils.WaitForFrames(1);

      switch (networkRole) {
        case NetworkRoleSelector.ListeningServer:
          manager.StartHost();
          break;
        case NetworkRoleSelector.Host:
          manager.StartServer();
          break;
        case NetworkRoleSelector.Client:
          manager.StartClient();
          break;
      }
    }
  }
}


