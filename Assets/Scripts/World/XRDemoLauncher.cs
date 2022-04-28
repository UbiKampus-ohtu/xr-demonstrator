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
    public string ip = "localhost";

    private NetworkManager manager;

    private void Awake() {
      manager = GetComponent<NetworkManager>();

      readCLIArguments();

      StartCoroutine(init());
    }

    private void readCLIArguments() {
      string [] args = System.Environment.GetCommandLineArgs();
      bool getIp = false;
      foreach (string argument in args) {
        if (getIp) {
          getIp = false;
          ip = argument;
        } else if (argument.Contains("-client")) {
          networkRole = NetworkRoleSelector.Client;
        } else if (argument.Contains("-listeningServer")) {
          networkRole = NetworkRoleSelector.ListeningServer;
        } else if (argument.Contains("-dedicatedServer")) {
          networkRole = NetworkRoleSelector.Host;
        } else if (argument.Contains("-ip")) {
          getIp = true;
        }
      }
    }

    IEnumerator init() {
      // wait for scene to initialize
      yield return CoroutineUtils.WaitForFrames(1);

      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = false;

      manager.networkAddress = ip;
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


