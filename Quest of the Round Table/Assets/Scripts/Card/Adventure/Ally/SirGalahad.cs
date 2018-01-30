using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirGalahad : Ally {

	//TODO
	private int empoweredBattlePoints;

	public SirGalahad() {
		battlePoints = 10;
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
