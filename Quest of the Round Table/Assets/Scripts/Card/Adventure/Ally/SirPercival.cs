using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirPercival : Ally {

	private int empoweredBattlePoints;

	public SirPercival() : base ("Sir Percival", 5, 0) {
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
