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

	protected void startBehaviour(){

		// 1) prompt user if they want to sponsor quest.

		if (owner.acceptQuest ())
			validateSponsor ();

		// 2) if yes, validate that they have the required num of foes
		//		else, loop through the rest of the players

		// 3) Player places desired foes onto board
		// 4) Player clicks "Ready button"
		// 5) card validation

		// 6) loop through players to see if they pass each round (with only their current rank points)


	}

	private bool validateSponsor(){
		
		//validate if current player has needed cards.
		List<Card> hand = owner.getHand();
		return true;
	}
		
}
