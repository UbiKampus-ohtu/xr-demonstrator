using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
  private Dictionary<string, Action<string>> eventDictionary;

  private static EventManager eventManager;

  public static EventManager instance {
    get {
      if (!eventManager) {
        eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;

        if (!eventManager) {
          Debug.Log("sfdljasklfjalsf");
        } else {
          eventManager.init();
        }
      }
      return eventManager;
    }
  }

  private void init() {
    if (eventDictionary == null) {
      eventDictionary = new Dictionary<string, Action<string>>();
    }
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
    if (eventManager == null) return;
    Action<string> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent -= listener;
      instance.eventDictionary[eventName] = thisEvent;
    }
  }

  public static void trigger(string eventName, string eventParameter) {
    Action<string> thisEvent = null;
    if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
      thisEvent.Invoke(eventParameter);
    }
  }
}
