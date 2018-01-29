using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Adventure {
	protected int battlePoints, bonusBattlePoints;
	private enum dominantFoe {};

	public int getBattlePoints() {
		return battlePoints;
	}
}
