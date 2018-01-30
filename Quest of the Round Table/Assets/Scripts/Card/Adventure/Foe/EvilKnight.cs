using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKnight : Foe {

	private int empoweredBattlePoints;

	public EvilKnight() {
	  battlePoints = 20;
		empoweredBattlePoints = 30;
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
