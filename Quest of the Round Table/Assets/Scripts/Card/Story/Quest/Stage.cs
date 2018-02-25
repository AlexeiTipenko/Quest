using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {
	private BoardManagerMediator board;

	private int currentStageNum, currentBid;
	private Adventure stageCard;
	private List<Weapon> weapons;

	private Quest quest;
    Player playerToPrompt;

	public Stage(Adventure stageCard, List<Weapon> weapons) {
		board = BoardManagerMediator.getInstance ();

		this.stageCard = stageCard;
		this.weapons = weapons;
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
        return totalCards;
	}

	public void prepare() {
		quest = (Quest)BoardManagerMediator.getInstance ().getCardInPlay ();

		if (stageCard.GetType ().IsSubclassOf (typeof(Foe))) {
			Debug.Log ("Is foe, going to player");
            Debug.Log("quest sponsor is: " + quest.getSponsor().getName());
			playerToPrompt = board.getNextPlayer (quest.getSponsor());
            Debug.Log("Current player is: " + playerToPrompt.getName());
            board.PromptFoe (playerToPrompt, currentStageNum);
		} else {
			//TODO: reveal visually;
			currentBid = ((Test)stageCard).getMinBidValue();
			Debug.Log ("Current bid is: " + currentBid);
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			promptTest ();
		}
	}

    public void promptFoeResponse(bool dropOut) { //TODO: I don't know how we can do last player in tournament
        if (playerToPrompt == quest.getSponsor()){
            playFoe();
        }
        else {
            if (!dropOut) {
                continueQuest(playerToPrompt);
            }
            else {
                Debug.Log("Dropped out");
                Player temp = playerToPrompt;
                playerToPrompt = quest.getNextPlayer(playerToPrompt);
                quest.removeParticipatingPlayer(temp);
                Debug.Log("Removed participant: " + quest.getPlayers().Count);
                continueQuest(temp);
            }
        }
	}

    public void promptFoeResp(bool dropOut) {
        if (!dropOut) {
            if (quest.getNextPlayer(playerToPrompt) != quest.getSponsor()) {
                Debug.Log("Sponsor is next");
                playerToPrompt = quest.getNextPlayer(playerToPrompt);
                board.PromptFoe(playerToPrompt, currentStageNum);
            }
            else{
                playFoe();
            }
        }
        else{
            Debug.Log("Drop out");
            if (quest.getNextPlayer(playerToPrompt) != quest.getSponsor()){
                Player temp = playerToPrompt;
                playerToPrompt = quest.getNextPlayer(playerToPrompt);
                quest.removeParticipatingPlayer(temp);
                board.PromptFoe(playerToPrompt, currentStageNum);
            }
            else {
                playFoe();
            }
        }
    }

    public void continueQuest(Player currPlayer){
        //Debug.Log("Current amount of players is: " + quest.getPlayers().Count);
        if (quest.getPlayers().Count <= 1)
        {
            Debug.Log("less than one player");
            //TODO: finish quest early somehow
        }
        else{
            //Debug.Log("Next player is now " + currPlayer.getName());
            //if (currPlayer != quest.getSponsor()) {
                Debug.Log("Next player isn't sponsor");
                board.PromptFoe(currPlayer, currentStageNum);
            //}
            //else {
            //    Debug.Log("Next player is sponsor");
            //    playFoe();
            //}
        }

    }

	private void promptTest() {
		if (isValidBidder ()) {
			board.PromptTest (playerToPrompt, currentBid);
		} else {
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
		if (bid == 0) {
			quest.removeParticipatingPlayer (playerToPrompt);
			if (quest.getPlayers ().Count == 1) {
				board.dealCardsToPlayer (quest.getPlayers()[0], 1);
				//TODO: discard specifically selected cards
				quest.playStage ();
			}
		} else {
			currentBid = bid;
		}
	}

	private void playFoe() {
        Debug.Log("playing foe");
		foreach (Player player in quest.getPlayers()) {
			int playerBattlePoints = player.getRank ().getBattlePoints ();
			List<Card> stageCards = player.getPlayArea ().getCards ();
			foreach (Card card in stageCards) {
				playerBattlePoints += ((Adventure) card).getBattlePoints ();
			}
			if (playerBattlePoints >= getTotalBattlePoints ()) {
                Debug.Log("player " + player.getName() + " past stage.");
				board.dealCardsToPlayer (player, 1);
				player.getPlayArea ().discardWeapons ();
				quest.playStage ();
			} else {
				quest.removeParticipatingPlayer (player);
				//TODO: somehow finish removing a player from the quest. Does this require more work?
			}
		}
	}
}
