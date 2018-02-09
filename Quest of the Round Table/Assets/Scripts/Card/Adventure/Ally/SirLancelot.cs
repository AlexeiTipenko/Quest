using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirLancelot : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirLancelot() : base ("Sir Lancelot", 15, 0) {
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

