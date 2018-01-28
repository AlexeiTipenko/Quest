using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : Adventure {

	private int battlePoints;
	private int bidPoints;

	public Amour() {
		battlePoints = 10;
		bidPoints = 1;
	}

	int getBattlePoints() {
		return battlePoints;
	}

	int getBidPoints() {
		return bidPoints;
	}


}
