using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Foe {

	private int empoweredBattlePoints;

	public Boar() {
		battlePoints = 5;
		empoweredBattlePoints = 15;
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
