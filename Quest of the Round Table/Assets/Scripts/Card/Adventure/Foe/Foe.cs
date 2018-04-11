using System;
using System.Collections.Generic;

[Serializable]
public abstract class Foe : Adventure {

	protected int battlePoints;
	protected int empoweredBattlePoints;

	public Foe (string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
        this.empoweredBattlePoints = battlePoints;
	}

	public override int getBattlePoints() {
		List<Type> dominantFoes = ((Quest)BoardManagerMediator.getInstance ().getCardInPlay ()).getDominantFoes ();
        if (dominantFoes.Contains (Type.GetType(cardImageName, true))) {
			return empoweredBattlePoints;

		}
		return battlePoints;
	}

    public int GetMinBattlePoints() {
        return battlePoints;
    }

}
