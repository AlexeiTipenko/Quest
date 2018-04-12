using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Quest : Story {
	BoardManagerMediator board;

	protected int currentStage, totalCardsCounter, numShieldsAwarded, numStages;
    protected List<Type> dominantFoes;
	protected List<Player> participatingPlayers;
	public static bool KingsRecognitionActive;
    List<Stage> stages;
    Player sponsor, playerToPrompt;
    Action action;

	protected Quest (string cardName, int numStages) : base (cardName) {
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
		sponsor.PromptSponsorQuest (this);
	}

	public void PromptSponsorQuestResponse (bool sponsorAccepted) {
		if (sponsorAccepted) {
			Logger.getInstance().info("Sponsor accepted: " + sponsor.getName());
            Debug.Log("Sponsor accepted: " + sponsor.getName());
            sponsor.SponsorQuest(this, true);
		} else {
			Logger.getInstance().trace("Sponsor declined: " + sponsor.getName());
			IncrementSponsor ();
		}
	}

    public Boolean IsValidQuest() {
        int minBattlePoints = 0;
        bool hasTest = false;
        for (int i = 0; i < numStages; i++)
        {
            GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
            List<Type> weaponsInStage = new List<Type>();
            bool hasFoe = false;
            bool currentStageHasTest = false;
            int currentBattlePoints = 0;
            foreach (Transform child in boardAreaFoe.transform)
            {
                Debug.Log("Evaluating new child: " + child.name);
                foreach (Adventure card in sponsor.GetHand())
                {
                    Debug.Log("Evaluating new card: " + card.GetCardName());
                    if (child.name == card.GetCardName())
                    {
                        Debug.Log("Found matching name");
                        if (card.IsTest())
                        {
                            if (hasTest)
                            {
                                Debug.Log("Quest setup failed.");
                                Logger.getInstance().warn("Quest setup failed.");
                                return false;
                            }
                            Debug.Log("Adding test to quest stage");
                            hasTest = true;
                            currentStageHasTest = true;
                            break;
                        }
                        else if (card.IsFoe())
                        {
                            if (currentStageHasTest || hasFoe)
                            {
                                Debug.Log("Quest setup failed.");
                                Logger.getInstance().warn("Quest setup failed.");
                                return false;
                            }
                            Debug.Log("Adding foe to quest stage");
                            hasFoe = true;
                            currentBattlePoints += card.getBattlePoints();
                            break;
                        }
                        else if (card.IsWeapon())
                        {
                            if (currentStageHasTest || weaponsInStage.Contains(card.GetType()))
                            {
                                Debug.Log("Quest setup failed.");
                                Logger.getInstance().warn("Quest setup failed.");
                                return false;
                            }
                            Debug.Log("Adding weapon to quest stage");
                            weaponsInStage.Add(card.GetType());
                            currentBattlePoints += card.getBattlePoints();
                            break;
                        }
                    }
                }
            }
            if (!currentStageHasTest)
            {
                Debug.Log("Min: " + minBattlePoints);
                Debug.Log("Curr: " + currentBattlePoints);
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
			sponsor.PromptSponsorQuest (this);
		}
	}

	public void SponsorQuestComplete(List<Stage> stages) {
        Debug.Log("Finished quest setup");
        this.stages = stages;
        foreach (Stage stage in stages) {
            Debug.Log("Stage " + stage.getStageNum());
            stage.SetParentQuest(this);
            foreach (Adventure card in stage.getCards()) {
                Debug.Log(card.GetCardName());
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
            playerToPrompt.PromptAcceptQuest(this);
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
				Debug.Log("Entered action in accepting quest");
                Action completeAction = () =>
                {
					Debug.Log("Entered completeAction");
					if (board.IsOnlineGame()) {
                        Debug.Log("Sending to others");
                        board.getPhotonView().RPC("PromptNextAcceptQuest", PhotonTargets.Others);
                    }
                    PromptNextAcceptQuest();
                };
				if (playerToPrompt.GetHand().Count > 12) {
					playerToPrompt.DiscardCards(action, completeAction);
				} else {
					PromptNextAcceptQuest();
				}
            };
            Logger.getInstance().info("Drawing adventure card for player: " + playerToPrompt.getName());
            playerToPrompt.DrawCards(1, action);
        } else {
            playerToPrompt = board.getNextPlayer(playerToPrompt);
            PromptAcceptQuest();
        }
	}

    public void PromptNextAcceptQuest() {
        playerToPrompt = board.getNextPlayer(playerToPrompt);
        PromptAcceptQuest();
    }

	public void PlayStage() {
		currentStage++;
		Debug.Log ("Playing stage: " + currentStage);
        foreach (Player player in board.getPlayers()) {
			Debug.Log ("Discarding weapons for player: " + player.getName());
			player.getPlayArea ().DiscardClass (typeof(Weapon));
        }
        if (currentStage < numStages && participatingPlayers.Count > 0) {
            getStage(currentStage).Prepare();
		} else {
			CompleteQuest ();
		}
	}

	public void CompleteQuest() {
		foreach (Player player in board.getPlayers()) {
			player.getPlayArea ().DiscardClass (typeof(Amour));
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
		action = () => {
			Action completeAction = () => {
				if (board.IsOnlineGame() && playerToPrompt.discarded) {
					playerToPrompt.toggleDiscarded(false);
					board.getPhotonView().RPC("nextTurn", PhotonTargets.Others);
				}				
				board.nextTurn();
			};
            if (playerToPrompt.GetHand().Count > 12) {
                playerToPrompt.DiscardCards(action, completeAction);
            }
            else {
                board.nextTurn();
            }
		};
		playerToPrompt.DrawCards(totalCardsCounter + numStages, action);
	}

	public bool ContainsOnlyValidCards(Player player) {
		List<Adventure> cards = BoardManager.GetPlayArea (player);
		foreach (Adventure card in cards) {
			if (!card.IsWeapon() && !card.IsAlly() && !card.IsAmour()) {
				return false;
			}
		}
		return true;
	}

	public void removeParticipatingPlayer(Player playerToRemove) {
		foreach (Player player in participatingPlayers) {
			if (player.getName () == playerToRemove.getName ()) {
				participatingPlayers.Remove (player);
				Logger.getInstance ().trace (player.getName() + " removed from quest");
				break;
			}
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
