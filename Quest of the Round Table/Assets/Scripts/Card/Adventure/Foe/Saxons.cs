using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxons : Foe {

	private int empoweredBattlePoints;

	public Saxons() {
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
