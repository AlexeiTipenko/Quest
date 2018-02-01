using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenGuinevere : Ally {
	
	public static int frequency = 1;
	private int empoweredBidPoints;

	public QueenGuinevere() : base ("Queen Guinevere", 0, 3) {
		empoweredBidPoints = 4;
	}

	public override int getBidPoints() {
		/*
		 * if (condition) {
		 *     return empoweredBidPoints;
		 * }
		*/
		return bidPoints;
	}
}
