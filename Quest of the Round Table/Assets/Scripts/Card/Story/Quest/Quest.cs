using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {

	protected int numStages;
	private enum dominantFoe {};
	private List<Stage> stages;
	Player sponsor, playerToPrompt;
	List<Player> playersParticipating, allPlayers;

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
		allPlayers = BoardManagerMediator.getInstance ().getPlayers ();
		promptSponsorQuest ();
	}

	private void promptSponsorQuest() {
		if (isValidSponsor ()) {
			BoardManagerMediator.getInstance ().promptSponsorQuest (sponsor);
		} else {
			incrementSponsor ();
		}
	}

	public void promptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			BoardManagerMediator.getInstance ().setupQuest (sponsor);
		} else {
			incrementSponsor ();
		}
	}

	private void incrementSponsor() {
		sponsor = BoardManagerMediator.getInstance ().getNextPlayer (sponsor);
		if (sponsor == owner) {
//				discard();
		} else {
			promptSponsorQuest ();
		}
	}

	public void setupQuestComplete() {
		playerToPrompt = BoardManagerMediator.getInstance ().getNextPlayer (sponsor);
		BoardManagerMediator.getInstance ().promptAcceptQuest (playerToPrompt);
	}

	public void promptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
			playersParticipating.Add (playerToPrompt);
		}
		playerToPrompt = BoardManagerMediator.getInstance ().getNextPlayer (playerToPrompt);
	}

	private bool isValidSponsor () {
		
		//validate if current player has needed cards.
		List<Card> hand = sponsor.getHand();

		int foeCount = 0;
		foreach (Card card in hand) {
			if (card.GetType().IsSubclassOf(typeof(Foe))) {
				foeCount++;
			}
		}
		return (foeCount >= numStages);
	}
}
