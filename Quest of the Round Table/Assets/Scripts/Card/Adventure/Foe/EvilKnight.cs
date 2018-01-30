using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKnight : Foe {

	private int empoweredBattlePoints;

	public EvilKnight() : base ("Evil Knight", 20) {
		empoweredBattlePoints = 30;
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
