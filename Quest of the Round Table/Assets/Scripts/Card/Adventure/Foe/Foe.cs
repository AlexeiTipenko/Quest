using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Foe : Adventure {

	protected int battlePoints;
	protected int empoweredBattlePoints;

	public Foe (string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
	}

	public override int getBattlePoints() {
		List<Type> dominantFoes = ((Quest)BoardManagerMediator.getInstance ().getCardInPlay ()).getDominantFoes ();
        if (dominantFoes.Contains (Type.GetType(cardImageName, true))) {
			return empoweredBattlePoints;
		}
		return battlePoints;
	}

}
