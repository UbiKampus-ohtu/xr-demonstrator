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
    public IEnumerator LocalPlayerGravityWorks() {
      GameObject player = GameObject.Find("LocalPlayer");

      Vector3 initialPosition = player.transform.position;
      float originalDistanceToOrigo = player.transform.position.magnitude;
      yield return new WaitForSeconds(0.5f);
      float distanceToOrigo = player.transform.position.magnitude;
      Assert.Less(distanceToOrigo, originalDistanceToOrigo);
      Assert.Less(distanceToOrigo, 0.2f);
    }
  }
}
