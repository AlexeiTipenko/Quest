using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {
	private BoardManagerMediator board;

	private int currentStageNum, currentBid;
	private Adventure stageCard;
	private List<Weapon> weapons;

	private Quest quest;
	private Player playerToPrompt;

	public Stage(Adventure stageCard, List<Weapon> weapons) {
		board = BoardManagerMediator.getInstance ();

		this.stageCard = stageCard;
		this.weapons = weapons;
	}

	public int getTotalBattlePoints() {
		int battlePoints = 0;
		if (stageCard.GetType ().IsSubclassOf (typeof(Foe))) {
			battlePoints += ((Foe)stageCard).getBattlePoints ();
            if (weapons != null) {
                foreach (Weapon weapon in weapons) {
                    battlePoints += weapon.getBattlePoints();
                }
            }
		}
		return battlePoints;
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
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			board.PromptFoe (playerToPrompt);
		} else {
			//TODO: reveal visually;
			currentBid = ((Test)stageCard).getMinBidValue();
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			promptTest ();
		}
	}

	public void promptFoeResponse(bool dropOut) {
		if (!dropOut) {
			playerToPrompt = quest.getNextPlayer (playerToPrompt);
			if (playerToPrompt != quest.getSponsor ()) {
				board.PromptFoe (playerToPrompt);
			} else {
				playFoe ();
			}
		} else {
			quest.removeParticipatingPlayer (playerToPrompt);
			if (quest.getPlayers ().Count < 1) {
				//TODO: finish quest early somehow
			}
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
		foreach (Player player in quest.getPlayers()) {
			int playerBattlePoints = player.getRank ().getBattlePoints ();
			List<Card> stageCards = player.getPlayArea ().getCards ();
			foreach (Card card in stageCards) {
				playerBattlePoints += ((Adventure) card).getBattlePoints ();
			}
			if (playerBattlePoints >= getTotalBattlePoints ()) {
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
