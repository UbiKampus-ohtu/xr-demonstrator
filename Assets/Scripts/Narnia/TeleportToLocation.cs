using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToLocation : MonoBehaviour {
	private CharacterController controller;
	private bool goingToNarnia;
	private Vector3 destination = new Vector3(100, 0, 100);

	private void Awake() {
		//goingToNarnia = false;
		//destination = new Vector3(100f, 0, 100f);
	}

	private void OnTriggerEnter(Collider other) {
			Transform player = other.transform.root;
			GoToNarnia(player);
			//goingToNarnia = true;
			//other.transform = destination;
		//}
	}

	private void GoToNarnia(Transform player) {
		player.position = destination; 
		//goingToNarnia = false;
	}

}
