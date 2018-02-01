using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirGawain : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirGawain() : base ("Sir Gawain", 10, 0) {
		empoweredBattlePoints = 20;
	}

	public new int getBattlePoints() {
		/*
		 * if (condition) {
		 *     return empoweredBattlePoints;
		 * }
		*/
		return battlePoints;
	}
}


