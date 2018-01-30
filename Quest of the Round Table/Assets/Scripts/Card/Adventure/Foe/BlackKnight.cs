using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKnight : Foe {

	public static int frequency = 3;
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
