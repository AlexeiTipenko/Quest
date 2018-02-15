using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Foe : Adventure {

	protected int battlePoints;
	
	private enum dominantFoe {};

	public Foe (string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
	}

	public override int getBattlePoints() {
		return battlePoints;
	}

}
