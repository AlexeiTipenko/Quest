using System;

[Serializable]
public abstract class Weapon : Adventure {

	protected int battlePoints;

	public Weapon(string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
	}

	public override int getBattlePoints() {
		return battlePoints;
	}
}
