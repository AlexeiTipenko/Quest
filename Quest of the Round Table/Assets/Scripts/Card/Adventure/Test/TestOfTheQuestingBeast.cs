using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOfTheQuestingBeast : Test {

	public static int frequency = 2;
	private int empoweredMinBidValue;

	public TestOfTheQuestingBeast() : base ("Test of the Questing Beast", 3) {
		empoweredMinBidValue = 4;
	}

	public new int getBidPoints() {
		/*
		 * if (condition) {
		 *     return empoweredMinBidPoints;
		 * }
		*/
		return minBidValue;
	}

}
