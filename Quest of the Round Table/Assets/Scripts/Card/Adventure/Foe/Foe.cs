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
		Card cardInPlay = BoardManagerMediator.getInstance ().getCardInPlay ();
		if (cardInPlay != null) {
			List<Type> dominantFoes = ((Quest)cardInPlay).getDominantFoes ();
			if (dominantFoes.Contains (this.GetType ())) {
				return empoweredBattlePoints;
			}
		}

		return battlePoints;
	}

}
