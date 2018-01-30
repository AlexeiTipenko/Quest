using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKnight : Foe {

	private int empoweredBattlePoints = 35;

	public BlackKnight() {
		battlePoints = 25;
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
