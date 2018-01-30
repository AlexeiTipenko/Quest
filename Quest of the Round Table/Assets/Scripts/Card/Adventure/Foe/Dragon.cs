using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Foe {

	private int empoweredBattlePoints;

	public Dragon() : base ("Dragon", 50) {
		empoweredBattlePoints = 70;
	}

	public override int getBattlePoints() {
		/*
		 * if (condition) {
		 *     return empoweredBattlePoints;
		 * }
		*/
		return battlePoints;
	}
}
