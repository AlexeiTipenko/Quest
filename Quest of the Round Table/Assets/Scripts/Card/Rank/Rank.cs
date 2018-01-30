using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank : Card {

	protected int battlePoints, shieldsToProgress;

	public Rank() { }


	public int getBattlePoints() {
		return battlePoints;
	}

	public int getShieldsToProgress() {
		return shieldsToProgress;
	}
}
