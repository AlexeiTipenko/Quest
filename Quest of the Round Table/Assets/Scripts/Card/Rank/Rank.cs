using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rank : Card {

	protected int battlePoints, shieldsToProgress;
	protected Rank nextRank;

	public Rank(string cardName, int battlePoints, int shieldsToProgress, Rank nextRank) : base (cardName) {
		this.battlePoints = battlePoints;
		this.nextRank = nextRank;
		this.shieldsToProgress = shieldsToProgress;
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