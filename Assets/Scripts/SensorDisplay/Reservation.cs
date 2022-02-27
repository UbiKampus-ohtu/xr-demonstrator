﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Reservation : IComparable<Reservation> {
	private string name;
	private DateTime dateStart;
	private DateTime dateEnd;

	public Reservation(string name, DateTime startDate, DateTime endDate) {
		this.name = name;
		this.dateStart = startDate;
		this.dateEnd = endDate;
	}

  private void Start() {
  }

  void Update() {
  }

	public DateTime getDateStart() {
		return dateStart;
	}

	public DateTime getDateEnd() {
		return dateEnd;
	}

	public int CompareTo(Reservation other) {
		if (other == null) return 1;

		if (other.getDateStart() <= dateStart) {
			return 1;
		} 

		return 0;
	}
}
