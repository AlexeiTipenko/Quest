using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirGalahad : Ally {

	private int empoweredBattlePoints;

	public SirGalahad() : base ("Sir Galahad", 10, 0) {
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
