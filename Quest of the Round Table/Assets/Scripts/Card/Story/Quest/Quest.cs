using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {

	protected int numStages;
	private enum dominantFoe {};
	private List<Stage> stages;
	Player sponsor;

	public Quest (string cardName, int numStages/*, enum dominantFoe*/) : base (cardName) {
		this.numStages = numStages;
		/*this.dominantFoe = dominantFoe;*/
	}

	public int getShieldsWon () {
		return numStages;
	}

	public override void startBehaviour () {
		Debug.Log ("Quest behaviour started");

		sponsor = owner;
		checkForValidSponsor ();

		BoardManagerData.getInstance ().promptQuest (sponsor);
	}

	public void sponsorQuest () {

	}

	private bool checkForValidSponsor () {
		
		//validate if current player has needed cards.
		List<Card> hand = sponsor.getHand();

		int foeCount = 0;
		foreach (Card card in hand) {
			if (typeof(Card).IsSubclassOf(typeof(Foe))) {
				foeCount++;
			}
		}

		if (foeCount >= numStages)
			return true;
		
		else
			return false;
	}
}
