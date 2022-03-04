using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour {
  public string URI = "localhost";
  public int port = 9000;

  private IPEndPoint serverEndPoint;
  private bool connected = false;
  private UdpClient udpClient;

  private Dictionary<string, Action<object>> eventDictionary;
  
  private static NetworkManager networkManager;
  public static NetworkManager instance {
    get {
      if (!networkManager) {
        networkManager = FindObjectOfType (typeof(NetworkManager)) as NetworkManager;
        if (!networkManager) {
          Debug.Log("sfdljasklfjalsf");
        } else {
          networkManager.init();
          networkManager.connect();
        }
      }
      return networkManager;
    }
  }

  private void init() {
    if (eventDictionary == null) {
      eventDictionary = new Dictionary<string, Action<object>>();
    }
  }

  private void connect() {
    if (connected) return;

    try {
      udpClient = new UdpClient(port);
      udpClient.BeginReceive(new AsyncCallback(onMessage), udpClient);
      connected = true;
    } catch (Exception err) {
      Debug.LogErrorFormat("Failed to initialize connection: {0}", err.ToString());
      connected = false;
      udpClient = null;
    }
  }

  private IPEndPoint getIPEndPoint() {
    IPAddress[] addressList = Dns.GetHostAddresses(URI);
    if (addressList.Length < 1) {
      throw new Exception("Cannot reach host");
    }
    return new IPEndPoint(addressList[0], port);
  }

  private int sendGeneralMessageSegment(byte[] data, int cursor) {
    string target = "";
    string payload = "";
    bool targetParsed = false;
    bool finished = false;

    while (cursor < data.Length && !targetParsed) {
      if (data[cursor] == '\n') {
        targetParsed = true;
      } else {
        target += (char)data[cursor];
      }
      cursor++;
    }

    while (cursor < data.Length - 1 && !finished) {
      if (data[cursor] == '\n') {
        trigger(target, payload);

        if (data[cursor + 1] == '\n') {
          finished = true;
          cursor++;
        } else {
          payload = "";
        }

      } else {
        payload += (char)data[cursor];
      }
      cursor++;
    }

    return cursor;
  }

  private void parseGeneralMessage(byte[] data) {
    int cursor = 1;
    while (cursor < data.Length) {
      cursor = sendGeneralMessageSegment(data, cursor);
    }
  }

  private float [] parsePlayerTransform(byte[] data, int cursor) {
    float [] transform = new float[7];
    Buffer.BlockCopy(data, cursor, transform, 0, 7 * 4);
    Debug.LogFormat("{0} {1} {2} {3} {4} {5} {6}", transform[0], transform[1], transform[2], transform[3], transform[4], transform[5], transform[6]);
    return transform;
  }

  private void parsePlayerData(byte[] data) {
    int cursor = 1;
    int previousPlayer = -1;
    int messageNumber = 0;

    while (cursor < data.Length) {
      int playerId = data[cursor];
      float [] transform = parsePlayerTransform(data, cursor + 1);

      if (playerId != previousPlayer) {
        messageNumber = 0;
        previousPlayer = playerId;
      } else {
        messageNumber++;
      }

      string eventId = String.Format("player {0} {1} transform", playerId, messageNumber);
      trigger(eventId, transform[0]);

      cursor += 7 * 4;
    }
  }

  private void onMessage(IAsyncResult message) {

    UdpClient socket = message.AsyncState as UdpClient;
    IPEndPoint sender = new IPEndPoint(0, 0);
    byte[] data = socket.EndReceive(message, ref sender);

    udpClient.BeginReceive(new AsyncCallback(onMessage), udpClient);

    Debug.LogFormat("Received {0} bytes", data.Length);

    if (data.Length == 0) {
      return;
    }
    if (data[0] == 1) {
      parseGeneralMessage(data);
    } else if (data[0] == 2) {
      parsePlayerData(data);
    }
  }

  public static void startListening(string eventName, Action<object> listener) {
    Action<object> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent += listener;
      instance.eventDictionary[eventName] = thisEvent;
    } else {
      thisEvent += listener;
      instance.eventDictionary.Add(eventName, thisEvent);
    }
  }

  public static void stopListening(string eventName, Action<object> listener) {
    if (networkManager == null) return;
    Action<object> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent -= listener;
      instance.eventDictionary[eventName] = thisEvent;
    }
  }

  private void trigger(string eventId, object payload) {
    Action<object> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventId, out thisEvent)) {
      thisEvent.Invoke(payload);
    }
  }

  private void OnApplicationQuit() {
    if (udpClient != null) {
      udpClient.Close();
    }
  }
}
