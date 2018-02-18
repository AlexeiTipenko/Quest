using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {
	private BoardManagerMediator board;

	protected int numStages, currentStage, totalCardsCounter, numShieldsAwarded;
	protected List<Type> dominantFoes;
	private List<Stage> stages;
	Player sponsor, playerToPrompt;
	List<Player> participatingPlayers;
	public static bool KingsRecognitionActive = false;

	public Quest (string cardName, int numStages) : base (cardName) {
		board = BoardManagerMediator.getInstance ();

		this.numStages = numStages;
	}

	public int getShieldsWon () {
		return numShieldsAwarded;
	}

	public List<Type> getDominantFoes() {
		return dominantFoes;
	}

	public override void startBehaviour () {
		Debug.Log ("Quest behaviour started");

		sponsor = owner;
		totalCardsCounter = 0;
		promptSponsorQuest ();
	}

	private void promptSponsorQuest() {
		if (isValidSponsor ()) {
			board.promptSponsorQuest (sponsor);
		} else {
			incrementSponsor ();
		}
	}

	public void promptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			board.setupQuest (sponsor);
		} else {
			incrementSponsor ();
		}
	}

	private void incrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
			//TODO: discard();
		} else {
			promptSponsorQuest ();
		}
	}

	public void setupQuestComplete(List<Stage> stages) {
		this.stages = stages;
		playerToPrompt = board.getNextPlayer (sponsor);
		board.promptAcceptQuest (playerToPrompt);
	}

	public void promptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
			participatingPlayers.Add (playerToPrompt);
		}
		playerToPrompt = board.getNextPlayer (playerToPrompt);
		if (playerToPrompt != sponsor) {
			board.promptAcceptQuest (playerToPrompt);
		} else {
			currentStage = -1;
			playStage ();
		}
	}

	public void playStage() {
		currentStage++;
		if (currentStage < numStages) {
			totalCardsCounter += stages [currentStage].getTotalCards ();
			stages [currentStage].prepare ();
		} else {
			completeQuest ();
		}
	}

	private void completeQuest() {
		foreach (Player player in board.getPlayers()) {
			player.getPlayArea ().discardAmours ();
		}
		numShieldsAwarded = numStages;
		if (KingsRecognitionActive) {
			numShieldsAwarded += 2;
			KingsRecognitionActive = false;
		}
		foreach (Player player in participatingPlayers) {
			player.incrementShields (numShieldsAwarded);
		}
		board.dealCardsToPlayer (sponsor, totalCardsCounter);
		board.nextTurn ();
	}

	private bool isValidSponsor () {
		List<Card> hand = sponsor.getHand();

		int validCardCount = 0;
		bool hasTest = false;
		foreach (Card card in hand) {
			if (card.GetType ().IsSubclassOf (typeof(Foe))) {
				validCardCount++;
			} else if (card.GetType ().IsSubclassOf (typeof(Test))) {
				hasTest = true;
			}
		}
		if (hasTest) {
			validCardCount++;
		}
		return (validCardCount >= numStages);
	}

	public void removeParticipatingPlayer(Player player) {
		if (participatingPlayers.Contains(player)) {
			participatingPlayers.Remove (player);
		}
	}

	public Player getSponsor() {
		return sponsor;
	}

	public Player getNextPlayer(Player previousPlayer) {
		int index = participatingPlayers.IndexOf (previousPlayer);
		if (index != -1) {
			return participatingPlayers [(index + 1) % participatingPlayers.Count];
		}
		return null;
	}

	public List<Player> getPlayers() {
		return participatingPlayers;
	}
}
