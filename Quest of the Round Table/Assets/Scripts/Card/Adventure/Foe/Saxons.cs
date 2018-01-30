using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxons : Foe {

	private int empoweredBattlePoints;

	public Saxons() : base("Saxons", 10) {
		empoweredBattlePoints = 20;
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
