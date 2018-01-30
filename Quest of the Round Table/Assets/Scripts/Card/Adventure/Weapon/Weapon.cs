using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Adventure {

	protected int battlePoints;

	public Weapon(string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
	}

	public int getBattlePoints() {
		return battlePoints;
	}
}
