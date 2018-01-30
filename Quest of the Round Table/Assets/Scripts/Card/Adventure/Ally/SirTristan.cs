using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirTristan : Ally {

	private int empoweredBattlePoints;

	public SirTristan() {
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
