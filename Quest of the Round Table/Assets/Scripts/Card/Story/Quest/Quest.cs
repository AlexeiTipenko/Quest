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
    Action action;

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
		Logger.getInstance ().trace ("dominantFoes are " + dominantFoes);
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
        if (sponsor.GetType() == typeof(AIPlayer)) {
            if (((AIPlayer)sponsor).GetStrategy().DoISponsorAQuest()) {
                //((AIPlayer)sponsor).GetStrategy().SponsorQuest();
                PromptSponsorQuestResponse(true);
            } else {
                PromptSponsorQuestResponse(false);
                //IncrementSponsor();
            }
        } else {
            board.PromptSponsorQuest(sponsor);
        }
	}

	public void PromptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			Logger.getInstance().info("Sponsor accepted: " + sponsor.getName());
            Debug.Log("Sponsor accepted: " + sponsor.getName());
            if (sponsor.GetType() == typeof(AIPlayer)) {
                ((AIPlayer)sponsor).GetStrategy().SponsorQuest();
            } else {
                board.SetupQuest(sponsor, "PREPARE YOUR QUEST\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages");
            }
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

	public void IncrementSponsor() {
		sponsor = board.getNextPlayer (sponsor);
		if (sponsor == owner) {
			board.nextTurn ();
		} else {
			PromptSponsorQuest ();
		}
	}

	public void SetupQuestComplete(List<Stage> stages) {
        Debug.Log("Finished quest setup.");
        this.stages = stages;
        foreach (Stage stage in stages) {
            Debug.Log("Stage " + stage.getStageNum());
            foreach (Card card in stage.getCards()) {
                Debug.Log(card.getCardName());
                if (sponsor.GetType() != typeof(AIPlayer)) {
                    sponsor.RemoveCard(card);
                }
            }
        }
        Logger.getInstance().info("Quest setup complete");
        playerToPrompt = board.getNextPlayer(sponsor);
        PromptAcceptQuest();
	}

    private void PromptAcceptQuest() {
        if (playerToPrompt != sponsor) {
            Debug.Log("Prompting " + playerToPrompt.getName() + " to accept quest");
            if (playerToPrompt.GetType() == typeof(AIPlayer)) {
                if (((AIPlayer)playerToPrompt).GetStrategy().DoIParticipateInQuest()) {
                    PromptAcceptQuestResponse(true);
                } else {
                    PromptAcceptQuestResponse(false);
                }
            } else {
                board.PromptAcceptQuest(playerToPrompt);
            }
        }
        else {
            currentStage = -1;
            numStages = stages.Count;
            foreach (Stage stage in stages)
            {
                totalCardsCounter += stage.getTotalCards();
            }
            Logger.getInstance().debug("Starting quest.");
            Debug.Log("Starting quest");
            PlayStage();
        }
    }

    public Stage getStage(int stageNum) {
        if (stages.Count == 0) {
            return null;
        }
        return stages[stageNum];
    }

    public Stage getCurrentStage() {
        if (currentStage >= numStages) {
            return stages[numStages - 1];
        }
        return stages[currentStage];
    }

	public void PromptAcceptQuestResponse(bool questAccepted) {
		if (questAccepted) {
			Logger.getInstance().debug(playerToPrompt.getName() + " has accepted to participate in the quest");
			participatingPlayers.Add (playerToPrompt);
            action = () => {
                board.TransferFromHandToPlayArea(playerToPrompt);
                playerToPrompt.RemoveCardsResponse();
                if (playerToPrompt.getHand().Count > 12)
                {
                    board.PromptCardRemoveSelection(playerToPrompt, action);
                }

                else
                {
                    playerToPrompt = board.getNextPlayer(playerToPrompt);
                    PromptAcceptQuest();
                }
            };
            if (playerToPrompt.getHand().Count + 1 > 12) {
                playerToPrompt.giveAction(action);
                board.dealCardsToPlayer(playerToPrompt, 1);
            } else {
                board.dealCardsToPlayer(playerToPrompt, 1);
                playerToPrompt = board.getNextPlayer(playerToPrompt);
                PromptAcceptQuest();
            }
        } else {
            playerToPrompt = board.getNextPlayer(playerToPrompt);
            PromptAcceptQuest();
        }
	}

	public void PlayStage() {
		currentStage++;
        foreach (Player player in board.getPlayers()) {
            player.getPlayArea().discardWeapons();
        }
        if (currentStage < numStages && participatingPlayers.Count > 0) {
            getStage(currentStage).prepare();
		} else {
			CompleteQuest ();
		}
	}

    public void PlayTest() {
        currentStage++;

    }

	public void CompleteQuest() {
		foreach (Player player in board.getPlayers()) {
			player.getPlayArea ().discardAmours ();
		}
		numShieldsAwarded = numStages;
		if (KingsRecognitionActive) {
			numShieldsAwarded += 2;
			KingsRecognitionActive = false;
		}
		Logger.getInstance ().trace ("numShieldsAwarded is " + numShieldsAwarded);
		foreach (Player player in participatingPlayers) {
			player.incrementShields (numShieldsAwarded);
		}
        Debug.Log("Quest complete");
        if (sponsor.getHand().Count + totalCardsCounter + numStages > 12) {
            action = () => {
                board.TransferFromHandToPlayArea(playerToPrompt);
                playerToPrompt.RemoveCardsResponse();
                if (playerToPrompt.getHand().Count > 12)
                {
                    board.PromptCardRemoveSelection(playerToPrompt, action);
                }

                else
                {
                    board.nextTurn();
                }
            };
            sponsor.giveAction(action);
            board.dealCardsToPlayer(sponsor, totalCardsCounter + numStages);
        } else {
            board.dealCardsToPlayer(sponsor, totalCardsCounter + numStages);
            board.nextTurn();
        }
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
