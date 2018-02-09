using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {

	protected int numStages;
	private enum dominantFoe {};
	private List<Stage> stages;

	public Quest (string cardName, int numStages/*, enum dominantFoe*/) : base (cardName) {
		this.numStages = numStages;
		/*this.dominantFoe = dominantFoe;*/
	}

	public int getShieldsWon() {
		return numStages;
	}
}
