using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Foe : Adventure {

	protected int battlePoints;
	protected int empoweredBattlePoints;

	public Foe (string cardName, int battlePoints) : base (cardName) {
		this.battlePoints = battlePoints;
        this.empoweredBattlePoints = battlePoints;
	}

	public override int getBattlePoints() {
		List<Type> dominantFoes = ((Quest)BoardManagerMediator.getInstance ().getCardInPlay ()).getDominantFoes ();
        Debug.Log("Empowered types for current quest: ");
        foreach (Type foe in dominantFoes) {
            Debug.Log(foe);
        }
        Debug.Log("This foe: " + this.GetType());
        if (dominantFoes.Contains (Type.GetType(cardImageName, true))) {
            Debug.Log(cardName + " battle points returned: " + empoweredBattlePoints);
			return empoweredBattlePoints;

		}
    Debug.Log(cardName + " battle points returned: " + battlePoints);
		return battlePoints;
	}

}
