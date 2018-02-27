using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {
	private BoardManagerMediator board;

	private int stageNum, currentBid;
	private Adventure stageCard;
	private List<Weapon> weapons;

	private Quest quest;
    Player playerToPrompt, originalPlayer;

	public Stage(Adventure stageCard, List<Weapon> weapons, int stageNum) {
		Logger.getInstance ().info ("Starting the Stage class");
		board = BoardManagerMediator.getInstance ();

		this.stageCard = stageCard;
		this.weapons = weapons;
        this.stageNum = stageNum;
	}

	public int getTotalBattlePoints() {
		int battlePoints = 0;
		if (stageCard.GetType ().IsSubclassOf (typeof(Foe))) {
			battlePoints += ((Foe)stageCard).getBattlePoints (); // here is breaking for some reason.
            if (weapons != null) {
                foreach (Weapon weapon in weapons) {
                    battlePoints += weapon.getBattlePoints();
                }
            }
		}
		Logger.getInstance ().trace ("The total battle points are " + battlePoints);
		return battlePoints;
	}

    public List<Card> getCards() {
        List<Card> cards = new List<Card>();
        cards.Add(stageCard);
        foreach (Weapon weapon in weapons) {
            cards.Add(weapon);
        }
        return cards;
    }

	public int getTotalCards() {
        int totalCards = 1;
        if (weapons != null) {
            totalCards += weapons.Count;
        }
		Logger.getInstance ().trace ("The total cards are " + totalCards);
        return totalCards;
	}

	public void prepare() {
		Logger.getInstance ().debug ("Prepare function has started");

		quest = (Quest)BoardManagerMediator.getInstance ().getCardInPlay ();

		if (stageCard.GetType ().IsSubclassOf (typeof(Foe))) {
			Logger.getInstance ().trace ("Stage card is subclass type of foe");
			Debug.Log ("Is foe, going to player");
            Debug.Log("quest sponsor is: " + quest.getSponsor().getName());
			playerToPrompt = board.getNextPlayer (quest.getSponsor());
            originalPlayer = playerToPrompt;
            Debug.Log("Current player is: " + playerToPrompt.getName());
            board.PromptFoe (playerToPrompt, stageNum);
		} else {
			//TODO: reveal visually;
			Logger.getInstance ().trace ("Stage card is NOT subclass type of foe");
			currentBid = ((Test)stageCard).getMinBidValue();
			Debug.Log ("Current bid is: " + currentBid);
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			promptTest ();
		}
	}


    public void promptFoeResponse(bool dropOut) {
        if (!dropOut) {
            playerToPrompt = quest.getNextPlayer(playerToPrompt);
            ContinueQuest(playerToPrompt);
        }
        else {
            Debug.Log("Dropped out");
            Player temp = playerToPrompt;
            playerToPrompt = quest.getNextPlayer(playerToPrompt);
            if (originalPlayer == temp) {
                originalPlayer = quest.getNextPlayer(originalPlayer);
            }
            Debug.Log("Removing player: " + temp.getName());
            quest.removeParticipatingPlayer(temp);
            Debug.Log("New total participant: " + quest.getPlayers().Count);
            Debug.Log("Next player: " + playerToPrompt.getName());
            ContinueQuest(playerToPrompt);
        }
	}


    public void ContinueQuest(Player currPlayer){
        //Debug.Log("Current amount of players is: " + quest.getPlayers().Count);
        if (quest.getPlayers().Count < 1)
        {
            Debug.Log("No quest participants left");
            quest.PlayStage();
        }
        else{
            Debug.Log("Original player: " + originalPlayer.getName());
            Debug.Log("Next player: " + currPlayer.getName());
            if (currPlayer != originalPlayer) {
                Debug.Log("Prompting player");
                board.PromptFoe(currPlayer, stageNum);
            }
            else {
                Debug.Log("All players have been prompted");
                PlayFoe();
            }
        }

    }

	private void promptTest() {
		Logger.getInstance ().debug ("prompting Test...");
		if (isValidBidder ()) {
			Logger.getInstance ().trace ("Bidder is valid; about to prompt test to " + playerToPrompt.getName());
			board.PromptTest (playerToPrompt, currentBid);
		} else {
			Logger.getInstance ().debug ("Removing Participating player...");
			quest.removeParticipatingPlayer (playerToPrompt);
			incrementBidder ();
		}
	}

	private bool isValidBidder() {
		return (playerToPrompt.getTotalAvailableBids () > currentBid);
	}

	private void incrementBidder() {
		playerToPrompt = quest.getNextPlayer (playerToPrompt);
		promptTest ();
	}

	public void promptTestResponse(int bid) { //TODO: maybe return should be the cards to be discarded? as well as bid number (to account for ally bids)
		Logger.getInstance ().debug ("prompting Test Response...");
		if (bid == 0) {
			quest.removeParticipatingPlayer (playerToPrompt);
			if (quest.getPlayers ().Count == 1) {
				board.dealCardsToPlayer (quest.getPlayers()[0], 1);
				//TODO: discard specifically selected cards
				quest.PlayStage ();
			}
		} else {
			Logger.getInstance ().trace ("Bid != 0");
			currentBid = bid;
		}
	}

	private void PlayFoe() {
        Debug.Log("Playing foe");
        Debug.Log("Num participating players: " + quest.getPlayers().Count);
        List<Player> playersToRemove = new List<Player>();
		foreach (Player player in quest.getPlayers()) {
			int playerBattlePoints = player.getRank ().getBattlePoints ();
			List<Card> stageCards = player.getPlayArea ().getCards ();
			foreach (Card card in stageCards) {
				playerBattlePoints += ((Adventure) card).getBattlePoints ();
			}
			if (playerBattlePoints >= getTotalBattlePoints ()) {
				Logger.getInstance ().trace ("playerBattlePoints >= getTotalbattlePoints");
                Debug.Log("Player " + player.getName() + " passed stage.");
				board.dealCardsToPlayer (player, 1);
				player.getPlayArea ().discardWeapons ();
			} else {
                Logger.getInstance ().trace ("Removing participating player " + player.getName());
                playersToRemove.Add(player);
			}
		}
        foreach (Player player in playersToRemove) {
            quest.removeParticipatingPlayer(player);
        }
        quest.PlayStage();
	}

    public int getStageNum() {
        return stageNum;
    }

    public Card getStageCard() {
        return stageCard;
    }
}
