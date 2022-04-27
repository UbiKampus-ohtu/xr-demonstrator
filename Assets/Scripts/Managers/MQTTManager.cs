using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class MQTTManager : MonoBehaviour {
  public string URI = "localhost";
  public int port = 9000;

  private IPEndPoint serverEndPoint;
  private bool connected = false;
  private UdpClient udpClient;

  private Dictionary<string, Action<object>> eventDictionary;
  
  private static MQTTManager mqttManager;
  public static MQTTManager instance {
    get {
      if (!mqttManager) {
        mqttManager = FindObjectOfType (typeof(MQTTManager)) as MQTTManager;
        if (!mqttManager) {
          Debug.Log("sfdljasklfjalsf");
        } else {
          mqttManager.init();
          mqttManager.connect();
        }
      }
      return mqttManager;
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

  private void parseMessage(byte[] data) {
    if (data.Length == 0) return;
    //datan pyörittely tähän, loppuun triggerit
  }

  private void onMessage(IAsyncResult message) {

    UdpClient socket = message.AsyncState as UdpClient;
    IPEndPoint sender = new IPEndPoint(0, 0);
    byte[] data = socket.EndReceive(message, ref sender);

    udpClient.BeginReceive(new AsyncCallback(onMessage), udpClient);

    parseMessage(data);
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
    if (mqttManager == null) return;
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
