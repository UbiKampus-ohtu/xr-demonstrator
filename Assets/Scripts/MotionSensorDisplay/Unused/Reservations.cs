using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservations : MonoBehaviour {
	private float timer;
	private string eventId;
	private DateTime tempDate;
	private Dictionary<string, Reservation> reservations;
	private List<Reservation> reservations2;

	private void Start() {
		//ChangeMaterial changeMat = GetComponentInChildren<ChangeMaterial>();
		reservations = new Dictionary<string, Reservation>();
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
			if (System.DateTime.Now >= reservations2[0].getDateStart()) {
				statusChange("green");
			} 

			timer = 0;
    }
  }

	private void addReservation(string reservationData) {
		//Convert.ToDateTime(String), DateTime.Parse() and DateTime.ParseExact()
		//tarkista varausten päällekkäisyydet?
		
		// tarvitaan erottaa ainakin id, start- ja endtime datasta
		//string[] splitResData = reservationData.Split(char.Parse(""));
		//string s = splitResData[0];

		//jotain jotain simsalabim, reservationDatasta saadaan start- ja enddate stringit?
		string sDate = "Mon Feb 28 09:23:53 2022 +0200"; // tms.
		string eDate = "2022-2-28 02:00:00"; // 02/28/2022 @ 8:31pm ?
		DateTime startDate = DateTime.Parse(sDate);
		DateTime endDate = DateTime.Parse(eDate);
		reservations2.Add(new Reservation("id", "dr. øtker", startDate, endDate));
		reservations.Add("id", new Reservation("id", "name", startDate, endDate));
	}

	private void removeReservation(string reservationData) {
		// tarvitaan erottaa reservationin id datasta
		string[] splitResData = reservationData.Split(char.Parse(""));
		if (reservations.ContainsKey(splitResData[0])) {
			reservations.Remove(splitResData[0]);
		}
		foreach (Reservation res in reservations2) {
			if(res.getId().Equals(splitResData[0])) {
				reservations2.Remove(res);
				break;
			}
		}
		// reservations.Remove(null);
		// reservations.RemoveAt(0);
	}

	private void statusChange(string status) {
		/* joku about to end huomautus keltaiselle */
		EventManager.trigger("change material", status); // tulee toimimaan väärin
		EventManager.trigger("reservation text", status);
	}
}