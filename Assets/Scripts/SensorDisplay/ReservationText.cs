using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReservationText : MonoBehaviour {
    public Text reservationText;

    private void OnEnable() {
        EventManager.startListening("reservation text", changeText);
    }

    private void OnDisable() {
        EventManager.stopListening("reservation text", changeText);
    }

    private void changeText(string reservationDetails) {
        //jotain vastaanotetun datan pyöritystä?
        reservationText.text = reservationDetails;
    }
}