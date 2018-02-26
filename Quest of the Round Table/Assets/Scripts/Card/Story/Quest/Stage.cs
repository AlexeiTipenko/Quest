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
		Logger.getInstance ().info ("Starting the Stage class");
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
		Logger.getInstance ().trace ("The total battle points are " + battlePoints);
		return battlePoints;
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
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			board.PromptFoe (playerToPrompt);
		} else {
			//TODO: reveal visually;
			Logger.getInstance ().trace ("Stage card is NOT subclass type of foe");
			currentBid = ((Test)stageCard).getMinBidValue();
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			promptTest ();
		}
	}

	public void promptFoeResponse(bool dropOut) {
		Logger.getInstance ().trace ("Prompting Foe response...");
		if (!dropOut) {
			playerToPrompt = quest.getNextPlayer (playerToPrompt);
			if (playerToPrompt != quest.getSponsor ()) {
				board.PromptFoe (playerToPrompt);
			} else {
				playFoe ();
			}
		} else {
			Logger.getInstance ().trace ("Removing participating player " + playerToPrompt.getName());
			quest.removeParticipatingPlayer (playerToPrompt);
			if (quest.getPlayers ().Count < 1) {
				Logger.getInstance ().trace ("Less than 1 player in stage left");
				//TODO: finish quest early somehow
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
				quest.playStage ();
			}
		} else {
			Logger.getInstance ().trace ("Bid != 0");
			currentBid = bid;
		}
	}

	private void playFoe() {
		Logger.getInstance ().trace ("Starting playFoe");
		foreach (Player player in quest.getPlayers()) {
			int playerBattlePoints = player.getRank ().getBattlePoints ();
			List<Card> stageCards = player.getPlayArea ().getCards ();
			foreach (Card card in stageCards) {
				playerBattlePoints += ((Adventure) card).getBattlePoints ();
			}
			if (playerBattlePoints >= getTotalBattlePoints ()) {
				Logger.getInstance ().trace ("playerBattlePoints >= getTotalbattlePoints");
				board.dealCardsToPlayer (player, 1);
				player.getPlayArea ().discardWeapons ();
				quest.playStage ();
			} else {
				Logger.getInstance ().trace ("Removing participating player " + player.getName());
				quest.removeParticipatingPlayer (player);
				//TODO: somehow finish removing a player from the quest. Does this require more work?
			}
		}
	}
}
