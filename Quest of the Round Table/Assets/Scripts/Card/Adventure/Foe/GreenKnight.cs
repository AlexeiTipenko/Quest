using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKnight : Foe {

	public static int frequency = 2;
	private int empoweredBattlePoints;

	public GreenKnight() : base ("Green Knight", 25) {
		empoweredBattlePoints = 40;
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
