using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPellinore : Ally {

	public static int frequency = 1;
	private int empoweredBidPoints;

	public KingPellinore() : base ("King Pellinore", 10, 0) {
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
