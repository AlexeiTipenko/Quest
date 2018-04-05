using System;
using UnityEngine;

[Serializable]
public abstract class Rank : Card {

	protected int battlePoints, shieldsToProgress;
	protected Rank nextRank;

	public Rank(string cardName, int battlePoints, int shieldsToProgress, Rank nextRank) : base (cardName) {
        Debug.Log("Setting battlepoints");
		this.battlePoints = battlePoints;
        Debug.Log("Set battlepoints is: " + this.battlePoints);
        Debug.Log("Setting next rank");
		this.nextRank = nextRank;
        Debug.Log("Creating shields");
		this.shieldsToProgress = shieldsToProgress;
        Debug.Log("Number of shields to progress is: " + this.shieldsToProgress);
	}


	public int getBattlePoints() {
		return battlePoints;
	}

	public int getShieldsToProgress() {
		return shieldsToProgress;
	}

	public Rank upgrade() {
		Logger.getInstance ().debug ("There has been an upgrade to " + nextRank.getCardName());
		return nextRank;
	}
}