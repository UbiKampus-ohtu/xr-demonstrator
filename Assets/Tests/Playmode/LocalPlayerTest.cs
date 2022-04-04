using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests {
  public class LocalPlayerTest {
    
    [OneTimeSetUp]
    public void LoadScene() {
      SceneManager.LoadScene("LocalPlayerTest");
    }

    [UnityTest]
    public IEnumerator PlayerGravityWorks() {
      yield return new WaitForSeconds(0.5f);
      GameObject player = GameObject.Find("Player [connId=0]");

      Vector3 initialPosition = player.transform.position;
      float originalDistanceToOrigo = initialPosition.magnitude;
      yield return new WaitForSeconds(1f);
      float distanceToOrigo = player.transform.position.magnitude;
      Assert.Less(distanceToOrigo, originalDistanceToOrigo);
      Assert.Less(distanceToOrigo, 0.5f);
    }

    [UnityTest]
    public IEnumerator PlayerTurningWorks() {
      yield return new WaitForSeconds(1.5f);
      GameObject player = GameObject.Find("Player [connId=0]");
      Vector3 originalForward = player.transform.forward;
      player.GetComponent<PlayerMovement>().testMode = true;

      Movement movement = player.GetComponent<Movement>();
      movement.turn(90f * (1 / movement.settings.mouseSensitivity));
      yield return CoroutineUtils.WaitForFrames(1);
      movement.turn(0);
      yield return CoroutineUtils.WaitForFrames(1);
      Assert.Less(Vector3.Dot(originalForward, player.transform.forward), 0.1f);
    }
  }
}
