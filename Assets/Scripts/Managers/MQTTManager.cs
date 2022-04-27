using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;

[Serializable]
public class RoomPayload {
  public string type;
  public int value;
  public override string ToString(){
    return type + " " + value;
  }
}
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

  private void parseMessage(byte[] bytes) {
    if (bytes.Length == 0) return;
    string data = Encoding.UTF8.GetString(bytes);

    MQTTMotionSensor motionMessage = JsonUtility.FromJson<MQTTMotionSensor>(data);
    if (MQTTTestData.isMotionSensor(motionMessage)) {
      RoomPayload roomPayload = new RoomPayload();
      roomPayload.type = "motionSensor";
      roomPayload.value = 1;
      trigger(motionMessage.topic, roomPayload);
      return;
    }

    MQTTReservation reservationMessage = JsonUtility.FromJson<MQTTReservation>(data);
    if (MQTTTestData.isReservation(reservationMessage)) {
      RoomPayload roomPayload = new RoomPayload();
      roomPayload.type = "reserved";
      roomPayload.value = reservationMessage.data.payload.currentTopic.Contains("VARATTU") ? 1 : 0;
      trigger(reservationMessage.topic, roomPayload);
      return;
    }
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

  private void trigger(string topic, object payload) {
    string [] subs = topic.Split('/');
    if (subs.Length < 2) return;
    string roomName = subs[1];

    Action<object> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(roomName, out thisEvent)) {
      thisEvent.Invoke(payload);
    }
  }

  private void OnApplicationQuit() {
    if (udpClient != null) {
      udpClient.Close();
    }
  }
}
