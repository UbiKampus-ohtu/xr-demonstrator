using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SerializablePayload {
  public string id;
  public string data;
}

public class NetworkManager : MonoBehaviour {
  public string URI = "localhost";
  public int port = 8000;

  private IPEndPoint serverEndPoint;
  private bool connected = false;
  private UdpClient udpClient;

  private Dictionary<string, Action<string>> eventDictionary;
  
  private static NetworkManager networkManager;
  public static NetworkManager instance {
    get {
      if (!networkManager) {
        networkManager = FindObjectOfType (typeof(NetworkManager)) as NetworkManager;
        if (!networkManager) {
          Debug.Log("sfdljasklfjalsf");
        } else {
          networkManager.init();
        }
      }
      return networkManager;
    }
  }

  private void Start() {
    if (!connected) {
      connect();
    }
  }

  private IPEndPoint getIPEndPoint() {
    IPAddress[] addressList = Dns.GetHostAddresses(URI);
    if (addressList.Length < 1) {
      throw new Exception("Cannot reach host");
    }
    return new IPEndPoint(addressList[0], port);
  }

  private void onMessage(IAsyncResult message) {
    UdpClient socket = message.AsyncState as UdpClient;
    IPEndPoint sender = new IPEndPoint(0, 0);
    byte[] data = socket.EndReceive(message, ref sender);

    udpClient.BeginReceive(new AsyncCallback(onMessage), udpClient);

    Debug.LogFormat("Received {0} bytes", data.Length);
    string payload = Encoding.UTF8.GetString(data);
    trigger(payload);
  }

  private void connect() {
    try {
      udpClient = new UdpClient(port);
      udpClient.BeginReceive(new AsyncCallback(onMessage), udpClient);
      connected = true;
      Debug.LogFormat("listening port {0}", port);
    } catch (Exception err) {
      Debug.LogErrorFormat("Failed to initialize connection: {0}", err.ToString());
      connected = false;
    }
  }

  private void init() {
    if (eventDictionary == null) {
      eventDictionary = new Dictionary<string, Action<string>>();
    }
    if (!connected) {
      connect();
    }
  }

  private string [] parsePayload(string payload) {
    SerializablePayload payloadJson = JsonUtility.FromJson<SerializablePayload>(payload);
    return new string []{payloadJson.id, payloadJson.data};
  }

  public static void startListening(string eventName, Action<string> listener) {
    Action<string> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent += listener;
      instance.eventDictionary[eventName] = thisEvent;
    } else {
      thisEvent += listener;
      instance.eventDictionary.Add(eventName, thisEvent);
    }
  }

  public static void stopListening(string eventName, Action<string> listener) {
    if (networkManager == null) return;
    Action<string> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent -= listener;
      instance.eventDictionary[eventName] = thisEvent;
    }
  }

  private void trigger(string payload) {
    string [] parsedPayload = parsePayload(payload);
    //Debug.LogFormat("id: {0}, data: {1}", parsedPayload[0], parsedPayload[1]);

    string eventId = parsedPayload[0];
    string data = parsedPayload[1];

    Action<string> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventId, out thisEvent)) {
      thisEvent.Invoke(data);
    }
  }
}
