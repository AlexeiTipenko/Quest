using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxonKnight : Foe {

	private int empoweredBattlePoints;

	public SaxonKnight() {
		battlePoints = 15;
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
