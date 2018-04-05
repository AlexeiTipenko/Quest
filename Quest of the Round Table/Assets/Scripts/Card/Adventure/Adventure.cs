using System;

[Serializable]
public abstract class Adventure : Card {

	protected Adventure(string cardName) : base (cardName) {
	
	}

	public virtual int getBattlePoints() {
		return 0;
	}

	public virtual int getBidPoints() {
		return 0;
	}

}
