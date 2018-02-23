using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {
	private BoardManagerMediator board;

    public int numStages, currentStage;
    protected int totalCardsCounter;
	protected List<Type> dominantFoes;
	private List<Stage> stages;
	Player sponsor, playerToPrompt;
	List<Player> participatingPlayers;

	public Quest (string cardName, int numStages) : base (cardName) {
		board = BoardManagerMediator.getInstance ();
		this.numStages = numStages;
        participatingPlayers = new List<Player>();
	}

	public int getShieldsWon () {
		return numStages;
	}

	public List<Type> getDominantFoes() {
		return dominantFoes;
	}

	public override void startBehaviour () {
		Debug.Log ("Quest behaviour started");

		sponsor = owner;
		totalCardsCounter = 0;
		PromptSponsorQuest ();
	}

	private void PromptSponsorQuest() {
		if (isValidSponsor ()) {
            Debug.Log("Requesting sponsor: " + sponsor.getName());
			board.PromptSponsorQuest (sponsor);
		} else {
            Debug.Log("Invalid sponsor: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

	public void PromptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
            Debug.Log("Sponsor accepted: " + sponsor.getName());
			board.SetupQuest (sponsor);
		} else {
            Debug.Log("Sponsor declined: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

	private void IncrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
            Debug.Log("All sponsors asked, none accepted.");
			//TODO: discard();
		} else {
			PromptSponsorQuest ();
		}
	}

	public void SetupQuestComplete() {
        this.stages = new List<Stage>(); //TODO: get the cards in the story card play area
        for (int i = 0; i < numStages; i++)
        {
            GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
            foreach (Transform child in boardAreaFoe.transform)
            {
                Debug.Log("card name is: " + child);
                foreach (Card card in sponsor.getHand())
                {
                    if (child.name == card.getCardName() && ((card.GetType().IsSubclassOf(typeof(Foe))) || (card.GetType().IsSubclassOf(typeof(Test)))))
                    {
                        Debug.Log("Ayy lemao this is a foe");
                    }
                }
            }
        }

        //TODO: get the card from player, make sure they match the ones played on the playarea, then keep doing setup quest
        // setup panels based on number of stages, then make sure each panel has attack less than another
        // use playarea for working with quest in playerplayarea (area where they drag cards when participating)
        Debug.Log("Finished quest setup.");
		playerToPrompt = board.getNextPlayer (sponsor);
		board.PromptAcceptQuest (playerToPrompt);
	}

	public void PromptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
            Debug.Log(playerToPrompt.getName() + " has accepted to participate in the quest.");
			participatingPlayers.Add (playerToPrompt);
		}
		playerToPrompt = board.getNextPlayer (playerToPrompt);
		if (playerToPrompt != sponsor) {
			board.PromptAcceptQuest (playerToPrompt);
		} else {
			currentStage = -1;
            numStages = stages.Count;
            Debug.Log("Starting quest.");
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
		foreach (Player player in participatingPlayers) {
			player.incrementShields (numStages);
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
