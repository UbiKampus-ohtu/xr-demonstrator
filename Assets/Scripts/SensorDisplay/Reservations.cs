using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservations : MonoBehaviour {
	private float timer;
	private string eventId;
	private DateTime tempDate;
	private List<Reservation> reservations;

	private void Start() {
		ChangeMaterial changeMat = GetComponentInChildren<ChangeMaterial>();
		reservations = new List<Reservation>();
  }

  private void OnEnable() {
		//eventId = string.Format("{0} motionSensor", gameObject.name);
    EventManager.startListening(eventId, addReservation);
		EventManager.startListening(eventId, removeReservation);
  }

	private void OnDisable() {
		EventManager.stopListening(eventId, addReservation);
		EventManager.stopListening(eventId, removeReservation);
	}

	private void Update() {
    timer += Time.deltaTime;
		// joku DateTime laskuri timerin sijaan?
    if (timer >= 10f) {
			if (System.DateTime.Now >= reservations[0].getDateStart()) {
				statusChange("green");
			} 

			timer = 0;
    }
  }

	private void addReservation(string reservationData) {
		//Convert.ToDateTime(String), DateTime.Parse() and DateTime.ParseExact()
		//jotain jotain simsalabim, reservationDatasta saadaan start- ja enddate stringit?
		string sDate = "2022-2-28 00:43:04"; // tms.
		string eDate = "2022-2-28 02:00:00";
		DateTime startDate = DateTime.Parse(sDate);
		DateTime endDate = DateTime.Parse(eDate);
		reservations.Add(new Reservation("dr. øtker", startDate, endDate));
	}

	private void removeReservation(string reservationData) {
		tempDate = DateTime.Parse(reservationData);
		// ??
		//reservations.Remove(null);
		//reservations.RemoveAt(0);
	}

	private void statusChange(string status) {
		/* joku about to end huomautus keltaiselle */
		EventManager.trigger("change material", status); // tulee toimimaan väärin
		EventManager.trigger("reservation text", status);
	}
}