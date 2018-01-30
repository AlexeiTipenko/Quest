using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnight : Foe {

	private int empoweredBattlePoints;

	public GreenKnight() {
		battlePoints = 25;
		empoweredBattlePoints = 40;
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
