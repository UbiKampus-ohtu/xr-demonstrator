using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Nopeustesti : MonoBehaviour {  
  private int score;
  private Button [] buttons;
  private Queue<int> buttonQueue;
  private int previousButtonIndex;
  private float timer;
  private float waitTime;
  public bool started = false;

  private UnityAction listener;

  AudioSource audioSource;
  public List<AudioClip> audioClips = new List<AudioClip>();

  private void addButton() {
    int newButtonIndex = Random.Range(0, 4);
    if (newButtonIndex != previousButtonIndex) {
      audioSource.Stop();
      audioSource.clip = audioClips[newButtonIndex];
      audioSource.Play();
      timer = 0;
      Button button = buttons[newButtonIndex];
      Button previousButton = buttons[previousButtonIndex];
      button.setEmission(true);
      previousButton.setEmission(false);
      buttonQueue.Enqueue(newButtonIndex);

      waitTime = getWaitTime();

      previousButtonIndex = newButtonIndex;
    } else {
      addButton();
    }
  }

  private float getWaitTime() {
    if (score <= 10) {
      return 0.565f;
    } else if (score <= 20) {
      return 0.513f;
    } else if (score <= 30) {
      return 0.461f;
    } else if (score <= 40) {
      return 0.410f;
    } else if (score <= 70) {
      return 0.390f;
    } else if (score <= 100) {
      return 0.360f;
    } else if (score <= 200) {
      return 0.306f;
    } else if (score <= 250) {
      return 0.250f;
    } else if (score <= 300) {
      return 0.230f;
    } else if (score <= 350) {
      return 0.210f;
    }
    return 0.190f;
  }

  private void OnEnable() {
    EventManager.startListening("nopeustesti pressed", buttonPressed);
    EventManager.startListening("nopeustesti start pressed", begin);
  }

  private void buttonPressed(string param) {
    if (!started) {
      return;
    }
    int lastButtonIndex = buttonQueue.Dequeue();
    if (int.Parse(param) != lastButtonIndex) {
      audioSource.Stop();
      started = false;
    } else {
      score++;
      if (buttonQueue.Count == 0) {
        addButton();
      }
    }
  }

  private void Start() {
    buttons = transform.GetComponentsInChildren<Button>();
    audioSource = gameObject.GetComponent<AudioSource>();
  }

  public void begin(string param) {
    if (started) {
      return;
    }
    foreach(var button in buttons) {
      button.setEmission(false);
    }
    score = 0;
    started = true;
    timer = 0f;
    previousButtonIndex = 0;
    waitTime = getWaitTime();
    buttonQueue = new Queue<int>();
  }

  private void Update() {
    if (!started) {
      return;
    }
    timer += Time.deltaTime;
    if (timer > waitTime) {
      addButton();
    }
  }
}
