using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : Adventure {

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
