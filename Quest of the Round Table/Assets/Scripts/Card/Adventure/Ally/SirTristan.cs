using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirTristan : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirTristan() : base ("Sir Tristan", 10, 0) {
		
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
