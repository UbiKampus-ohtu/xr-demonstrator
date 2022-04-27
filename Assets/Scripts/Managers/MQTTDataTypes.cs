[System.Serializable]
public class MQTTMotionSensor {
  [System.Serializable]
  public class MQTTMotionSensorData {
    [System.Serializable]
    public class MQTTMotionSensorPayload {
      public string type;
      public string roomName;
      public string roomId;
      public int timestamp;
    }
    public MQTTMotionSensorPayload payload;
  }
  public string topic;
  public MQTTMotionSensorData data;
}

[System.Serializable]
public class MQTTReservation {
  [System.Serializable]
  public class MQTTReservationData {
    [System.Serializable]
    public class MQTTReservationPayload {
      public long currentEndTime;
      public long currentStartTime;
      public string currentTopic;
    }
    public MQTTReservationPayload payload;
  }
  public string topic;
  public MQTTReservationData data;
}

public static class MQTTTestData {
  public static bool isMotionSensor(object data) {
    MQTTMotionSensor testData = data as MQTTMotionSensor;
    if (testData == null) return false;
    return testData.topic != null && testData.data.payload.type != null;
  }

  public static bool isReservation(object data) {
    MQTTReservation testData = data as MQTTReservation;
    if (testData == null) return false;
    return testData.topic != null && testData.data.payload.currentTopic != null;
  }
}