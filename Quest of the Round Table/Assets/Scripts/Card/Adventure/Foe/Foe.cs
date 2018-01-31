using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Foe : Adventure {

	protected int battlePoints;
	
	private enum dominantFoe {};

	public Foe (string cardName, int battlePoints) : base (cardName) {
		
	}

	public virtual int getBattlePoints() {
		return battlePoints;
	}

}
