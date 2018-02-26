using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {
	private BoardManagerMediator board;

	protected int currentStage, totalCardsCounter, numShieldsAwarded;
	public int numStages;
	protected List<Type> dominantFoes;
	private List<Stage> stages;
	Player sponsor, playerToPrompt;
	List<Player> participatingPlayers;
	public static bool KingsRecognitionActive = false;

	public Quest (string cardName, int numStages) : base (cardName) {
		Logger.getInstance ().info ("Initializing a Quest");

		board = BoardManagerMediator.getInstance ();
		this.numStages = numStages;
        participatingPlayers = new List<Player>();
	}

	public int getShieldsWon () {
		Logger.getInstance ().trace ("numShieldsAwarded is " + numShieldsAwarded);
		return numShieldsAwarded;
	}

	public List<Type> getDominantFoes() {
		Logger.getInstance ().trace ("dominantFoes are " + dominantFoes.ToString());
		return dominantFoes;
	}

	public override void startBehaviour () {
		Logger.getInstance().info ("Quest behaviour started");

		sponsor = owner;
		totalCardsCounter = 0;
		PromptSponsorQuest ();
	}

	private void PromptSponsorQuest() {
		if (isValidSponsor ()) {
			Logger.getInstance().trace("Requesting sponsor: " + sponsor.getName());
			board.PromptSponsorQuest (sponsor);
		} else {
			Logger.getInstance().trace("Invalid sponsor: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

	public void PromptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			Logger.getInstance().trace("Sponsor accepted: " + sponsor.getName());
            Action action = () => {
                ((Quest)BoardManagerMediator.getInstance().getCardInPlay()).SetupQuestComplete();
            };
			board.SetupQuest (sponsor, action);
		} else {
			Logger.getInstance().trace("Sponsor declined: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

	private void IncrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
			Logger.getInstance().trace("All sponsors asked, none accepted.");
			//TODO: discard();
		} else {
			PromptSponsorQuest ();
		}
	}

	public void SetupQuestComplete() {
		Logger.getInstance().info("Setting up the Quest is complete");
        this.stages = new List<Stage>(); //TODO: get the cards in the story card play area
        for (int i = 0; i < numStages; i++)
        {
            GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
            foreach (Transform child in boardAreaFoe.transform)
            {
				Logger.getInstance().trace("card name is: " + child);
                foreach (Card card in sponsor.getHand())
                {
                    if (child.name == card.getCardName() && card.GetType().IsSubclassOf(typeof(Test)))
                    {
						Logger.getInstance().trace("This is Test");
                    }
                    else if(child.name == card.getCardName() && card.GetType().IsSubclassOf(typeof(Foe))){
						Logger.getInstance().trace("This is Foe");
                    }
                }
            }
        }

        //TODO: get the card from player, make sure they match the ones played on the playarea, then keep doing setup quest
        // setup panels based on number of stages, then make sure each panel has attack less than another
        // use playarea for working with quest in playerplayarea (area where they drag cards when participating)
		Logger.getInstance().info("Finished Quest setup");
		playerToPrompt = board.getNextPlayer (sponsor);
		board.PromptAcceptQuest (playerToPrompt);
	}

	public void PromptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
			Logger.getInstance().debug(playerToPrompt.getName() + " has accepted to participate in the quest.");
			participatingPlayers.Add (playerToPrompt);
		}
		playerToPrompt = board.getNextPlayer (playerToPrompt);
		if (playerToPrompt != sponsor) {
			board.PromptAcceptQuest (playerToPrompt);
		} else {
			currentStage = -1;
            numStages = stages.Count;
			Logger.getInstance().debug("Starting quest.");
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
        Debug.Log("Complete Quest");
        BoardManager.DestroyStage(numStages);
		board.nextTurn ();
	}

	private bool isValidSponsor () {
		Logger.getInstance().debug("Starting isValidSponsor.");
		List<Card> hand = sponsor.getHand();
		int validCardCount = 0;
		bool hasTest = false;
		foreach (Card card in hand) {
			if (card.GetType ().IsSubclassOf (typeof(Foe))) {
				validCardCount++;
				Logger.getInstance().trace("Increasing validCardCount is now " + validCardCount);
			} else if (card.GetType ().IsSubclassOf (typeof(Test))) {
				hasTest = true;
				Logger.getInstance().trace("hasTest is now true");
			}
		}
		if (hasTest) {
			validCardCount++;
			Logger.getInstance().trace("Increasing validCardCount is now " + validCardCount);
		}
		return (validCardCount >= numStages);
	}

	public void removeParticipatingPlayer(Player player) {
		Logger.getInstance().debug("Starting removeParticipatingPlayer function on " + player.getName());
		if (participatingPlayers.Contains(player)) {
			participatingPlayers.Remove (player);
			Logger.getInstance ().trace ("Removed the player.");
		}
	}

	public Player getSponsor() {
		Logger.getInstance ().trace ("getSponsor; the sponsor is " + sponsor.getName());
		return sponsor;
	}

	public Player getNextPlayer(Player previousPlayer) {
		Logger.getInstance().debug("Starting removeParticipatingPlayer function on " + previousPlayer.getName());
		int index = participatingPlayers.IndexOf (previousPlayer);
		if (index != -1) {
			return participatingPlayers [(index + 1) % participatingPlayers.Count];
		}
		Logger.getInstance ().trace ("No next player.");
		return null;
	}

	public List<Player> getPlayers() {
		return participatingPlayers;
	}
}
