using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKnight : Foe {

	private int empoweredBattlePoints;

	public BlackKnight() : base("Black Knight", 25) {
		empoweredBattlePoints = 35;
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
