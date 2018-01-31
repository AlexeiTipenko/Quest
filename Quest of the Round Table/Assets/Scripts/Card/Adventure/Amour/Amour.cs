using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : Adventure {

	public static int frequency = 8;
	private int battlePoints, bidPoints;

	public Amour() : base ("Amour") {
		battlePoints = 10;
		bidPoints = 1;
	}

	public int getBattlePoints() {
		return battlePoints;
	}

	public int getBidPoints() {
		return bidPoints;
	}

}
