using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenIseult : Ally {

	public static int frequency = 1;
	private int empoweredBidPoints;

	public QueenIseult() : base ("Queen Iseult", 0, 2) {
		empoweredBidPoints = 4;
	}

	public new int getBidPoints() {
		/*
		 * if (condition) {
		 *     return empoweredBidPoints;
		 * }
		*/
		return bidPoints;
	}
}
