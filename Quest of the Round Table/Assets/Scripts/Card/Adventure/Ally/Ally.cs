using System;

[Serializable]
public abstract class Ally : Adventure {

	protected int battlePoints, bidPoints;

	public Ally(string cardName) : base (cardName) {
		battlePoints = 0;
		bidPoints = 0;
	}

	public Ally(string cardName, int battlePoints, int bidPoints) : base (cardName) {
		this.battlePoints = battlePoints;
		this.bidPoints = bidPoints;
	}

	public override int getBattlePoints() {
		return battlePoints;
	}

	public override int getBidPoints() {
		return bidPoints;
	}

}
