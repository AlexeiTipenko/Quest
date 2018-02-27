using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {
	private BoardManagerMediator board;

	protected int currentStage, totalCardsCounter, numShieldsAwarded;
	private int numStages;
	protected List<Type> dominantFoes;
	private List<Stage> stages;
	Player sponsor, playerToPrompt;
	List<Player> participatingPlayers;
	public static bool KingsRecognitionActive = false;

	public Quest (string cardName, int numStages) : base (cardName) {

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
        Logger.getInstance().info ("Started Quest behaviour");

		sponsor = owner;
		totalCardsCounter = 0;
		PromptSponsorQuest ();
	}

	private void PromptSponsorQuest() {
		if (isValidSponsor ()) {
			board.PromptSponsorQuest (sponsor);
		} else {
			Logger.getInstance().warn("Invalid sponsor: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

	public void PromptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			Logger.getInstance().info("Sponsor accepted: " + sponsor.getName());
            Debug.Log("Sponsor accepted: " + sponsor.getName());
            board.SetupQuest (sponsor, "PREPARE YOUR QUEST\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages");
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
                        if (!hasTest) {
                            hasTest = true;
                            currentStageHasTest = true;
                            break;
                        } else {
                            Debug.Log("Quest setup failed due to multiple tests.");
                            Logger.getInstance().warn("Quest setup failed due to multiple tests");
                            return false;
                        }
                    }
                    else if (child.name == card.getCardName() && cardType.IsSubclassOf(typeof(Foe))) {
                        if (currentStageHasTest) {
                            Debug.Log("Quest setup failed due to test existing in stage with foe/weapon.");
                            Logger.getInstance().warn("Quest setup failed due to test existing in stage with foe/weapon");
                            return false;
                        }
                        if (!hasFoe) {
                            hasFoe = true;
                            currentBattlePoints += ((Foe)card).getBattlePoints();
                            break;
                        }
                        else {
                            Debug.Log("Quest setup failed due to multiple foes in a stage.");
                            Logger.getInstance().warn("Quest setup failed due to multiple foes in a stage");
                            return false;
                        }
                    }
                    else if (child.name == card.getCardName() && cardType.IsSubclassOf(typeof(Weapon))) {
                        if (currentStageHasTest) {
                            Debug.Log("Quest setup failed due to test existing in stage with foe/weapon.");
                            Logger.getInstance().warn("Quest setup failed due to test existing in stage with foe/weapon");
                            return false;
                        }
                        if (!weaponsInStage.Contains(cardType)) {
                            weaponsInStage.Add(cardType);
                            currentBattlePoints += ((Weapon)card).getBattlePoints();
                            break;
                        } else {
                            Debug.Log("Quest setup failed due to multiple weapons of the same type in a stage.");
                            Logger.getInstance().warn("Quest setup failed due to multiple weapons of the same type in a stage");
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
                    Logger.getInstance().warn("Quest setup failed due to battle points");
                    return false;
                }
            }
        }
        return true;
    }

	private void IncrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
			board.nextTurn ();
		} else {
			PromptSponsorQuest ();
		}
	}

	public void SetupQuestComplete(List<Stage> stages) {
        this.stages = stages;
        foreach (Stage stage in stages) {
            foreach (Card card in stage.getCards()) {
                sponsor.RemoveCard(card);
            }
        }
        Debug.Log("Finished quest setup.");
        Logger.getInstance().info("Quest setup complete");
		playerToPrompt = board.getNextPlayer (sponsor);
		board.PromptAcceptQuest (playerToPrompt);
	}

    public Stage getStage(int stageNum) {
        if (stages.Count == 0) {
            return null;
        }
        return stages[stageNum];
    }

    public Stage getCurrentStage() {
        return stages[currentStage];
    }

	public void PromptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
			Logger.getInstance().debug(playerToPrompt.getName() + " has accepted to participate in the quest");
			participatingPlayers.Add (playerToPrompt);
		}
		playerToPrompt = board.getNextPlayer (playerToPrompt);
		if (playerToPrompt != sponsor) {
			board.PromptAcceptQuest (playerToPrompt);
		} else {
			currentStage = -1;
            numStages = stages.Count;
            foreach (Stage stage in stages) {
                totalCardsCounter += stage.getTotalCards();
            }
			Logger.getInstance().debug("Starting quest.");
            Debug.Log("Starting quest");
			PlayStage ();
		}
	}

	public void PlayStage() {
		currentStage++;
        if (currentStage < numStages && participatingPlayers.Count > 0) {
            getStage(currentStage).prepare();
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
        Debug.Log("Quest complete");
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
            Logger.getInstance ().trace (player.getName() + " removed from quest");
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
