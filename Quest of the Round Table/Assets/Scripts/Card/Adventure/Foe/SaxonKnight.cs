using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxonKnight : Foe {

	private int empoweredBattlePoints;

	public SaxonKnight() : base("Saxon Knight", 15) {
		empoweredBattlePoints = 25;
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
