using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRInputs : MonoBehaviour {
  private InputDevice hmd, leftHand, rightHand;
  public bool xrEnabled = false;

  public static bool isXrEnabled() {
    return UnityEngine.XR.XRSettings.enabled;
  }

  private InputDevice findHand(bool isLeft) {
    var devices = new List<InputDevice>();
    InputDeviceCharacteristics handness = isLeft ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right;
    UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(handness, devices);
    return devices[0];
  }

  private InputDevice findHMD() {
    var devices = new List<InputDevice>();
    UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, devices);
    return devices[0];
  }

  private void recenter() {
    hmd.subsystem.TryRecenter();
  }

  void Start() {
    xrEnabled = UnityEngine.XR.XRSettings.enabled;
    if (!xrEnabled) {
      return;
    }
    hmd = findHMD();
    leftHand = findHand(true);
    rightHand = findHand(false);
    recenter();
  }

  private Vector3 getDeviceLocalPosition(InputDevice device) {
    Vector3 localPosition;
    device.TryGetFeatureValue(CommonUsages.devicePosition, out localPosition);
    return localPosition;
  }

  private Quaternion getDeviceLocalRotation(InputDevice device) {
    Quaternion localRotation;
    device.TryGetFeatureValue(CommonUsages.deviceRotation, out localRotation);
    return localRotation;
  }

  public Vector3 getHMDLocalPosition() {  
    return getDeviceLocalPosition(hmd);
  }

  public Quaternion getHMDLocalRotation() {
    return getDeviceLocalRotation(hmd);
  }

  public Vector3 getHandLocalPosition(bool isLeft) {
    InputDevice hand = isLeft ? leftHand : rightHand;
    return getDeviceLocalPosition(hand);
  }

  public Quaternion getHandLocalRotation(bool isLeft) {
    InputDevice hand = isLeft ? leftHand : rightHand;
    return getDeviceLocalRotation(hand);
  }
}
