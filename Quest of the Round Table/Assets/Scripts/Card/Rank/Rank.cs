using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rank : Card {

	protected int battlePoints, shieldsToProgress;
	protected Rank nextRank;

	public Rank(string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
		this.nextRank = null;
	}


	public int getBattlePoints() {
		return battlePoints;
	}

	public int getShieldsToProgress() {
		return shieldsToProgress;
	}

	public Rank upgrade() {
		return nextRank;
	}
}
