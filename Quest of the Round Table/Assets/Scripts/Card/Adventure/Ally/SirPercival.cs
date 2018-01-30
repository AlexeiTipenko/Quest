using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirPercival : Ally {

	private int empoweredBattlePoints;

	public SirPercival() {
		battlePoints = 5;
		empoweredBattlePoints = 25;
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
