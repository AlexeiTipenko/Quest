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
        dominantFoes = new List<Type>();
        stages = new List<Stage>();
	}

	public int getShieldsWon () {
		Logger.getInstance ().trace ("numShieldsAwarded is " + numShieldsAwarded);
		return numShieldsAwarded;
	}

	public List<Type> getDominantFoes() {
		Logger.getInstance ().trace ("dominantFoes are " + dominantFoes.ToString());
		return dominantFoes;
	}

    public int getNumStages() {
        return numStages;
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
            Debug.Log("Sponsor accepted: " + sponsor.getName());
            board.SetupQuest (sponsor, "Prepare your quest using a combination of foes(and weapons) and a test.");
		} else {
			Logger.getInstance().trace("Sponsor declined: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

    public Boolean isValidQuest() {
        int minBattlePoints = 0;
        bool hasTest = false;
        for (int i = 0; i < numStages; i++) {
            GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
            List<Type> weaponsInStage = new List<Type>();
            bool hasFoe = false;
            bool currentStageHasTest = false;
            int currentBattlePoints = 0;
            foreach (Transform child in boardAreaFoe.transform) {
                Debug.Log("card name is: " + child.name);
                foreach (Card card in sponsor.getHand()) {
                    Type cardType = card.GetType();
                    if (child.name == card.getCardName() && cardType.IsSubclassOf(typeof(Test))) {
                        Debug.Log("This is Test");
                        if (!hasTest) {
                            hasTest = true;
                            currentStageHasTest = true;
                            break;
                        } else {
                            Debug.Log("Quest setup failed due to multiple tests.");
                            return false;
                        }
                    }
                    else if (child.name == card.getCardName() && cardType.IsSubclassOf(typeof(Foe))) {
                        Debug.Log("This is Foe");
                        if (currentStageHasTest) {
                            Debug.Log("Quest setup failed due to test existing in stage with foe/weapon.");
                            return false;
                        }
                        if (!hasFoe) {
                            hasFoe = true;
                            currentBattlePoints += ((Foe)card).getBattlePoints();
                            break;
                        }
                        else {
                            Debug.Log("Quest setup failed due to multiple foes in a stage.");
                            return false;
                        }
                    }
                    else if (child.name == card.getCardName() && cardType.IsSubclassOf(typeof(Weapon))) {
                        if (currentStageHasTest) {
                            Debug.Log("Quest setup failed due to test existing in stage with foe/weapon.");
                            return false;
                        }
                        if (!weaponsInStage.Contains(cardType)) {
                            weaponsInStage.Add(cardType);
                            currentBattlePoints += ((Weapon)card).getBattlePoints();
                            break;
                        } else {
                            Debug.Log("Quest setup failed due to multiple weapons of the same type in a stage.");
                            return false;
                        }
                    }
                }
            }
            if (!currentStageHasTest) {
                if (currentBattlePoints > minBattlePoints) {
                    minBattlePoints = currentBattlePoints;
                }
                else {
                    Debug.Log("Quest setup failed due to battle points.");
                    return false;
                }
            }
        }
        return true;
    }

	private void IncrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
			Logger.getInstance().trace("All sponsors asked, none accepted.");
			//TODO: discard();
			board.nextTurn ();
		} else {
			PromptSponsorQuest ();
		}
	}

	public void SetupQuestComplete(List<Stage> stages) {
            Logger.getInstance().info("Setting up the Quest is complete");
        this.stages = stages;
        foreach (Stage stage in stages) {
            foreach (Card card in stage.getCards()) {
                sponsor.RemoveCard(card);
            }
        }
        Debug.Log("Finished quest setup.");
        Logger.getInstance().info("Finished Quest setup");
		playerToPrompt = board.getNextPlayer (sponsor);
		board.PromptAcceptQuest (playerToPrompt);
	}

    public Stage getStage(int stageNum) {
        if (stages.Count == 0) {
            return null;
        }
        return stages[stageNum];
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
            Debug.Log("Starting quest.");
			PlayStage ();
		}
	}

	public void PlayStage() {
		currentStage++;
        if (currentStage < numStages && participatingPlayers.Count > 0) {
			totalCardsCounter += stages [currentStage].getTotalCards ();
			stages [currentStage].prepare ();
		} else {
			CompleteQuest ();
		}
	}

	private void CompleteQuest() {
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
