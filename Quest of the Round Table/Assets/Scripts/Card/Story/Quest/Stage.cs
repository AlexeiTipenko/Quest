using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage {
	private BoardManagerMediator board;

	private int currentStageNum;
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
			foreach (Weapon weapon in weapons) {
				battlePoints += weapon.getBattlePoints ();
			}
		}
		return battlePoints;
	}

	public void prepare() {
		quest = (Quest)BoardManagerMediator.getInstance ().getCardInPlay ();

		if (stageCard.GetType ().IsSubclassOf (typeof(Foe))) {
			playerToPrompt = quest.getNextPlayer (quest.getSponsor ());
			board.promptStage (playerToPrompt);
		} else {
			//TODO: reveal();
		}
	}

	public void promptStageResponse(bool dropOut) {
		if (!dropOut) {
			playerToPrompt = quest.getNextPlayer (playerToPrompt);
			if (playerToPrompt != quest.getSponsor ()) {
				board.promptStage (playerToPrompt);
			} else {
				play ();
			}
		} else {
			quest.removeParticipatingPlayer (playerToPrompt);
			if (quest.getPlayers ().Count < 1) {
				//TODO: finish quest somehow
			}
		}
	}

	private void play() {
		foreach (Player player in quest.getPlayers()) {
			int playerBattlePoints = player.getRank ().getBattlePoints ();
			List<Card> stageCards = player.getPlayArea ().getCards ();
			foreach (Card card in stageCards) {
				playerBattlePoints += ((Adventure) card).getBattlePoints ();
			}
			if (playerBattlePoints >= getTotalBattlePoints ()) {
				board.dealCardsToPlayer (player, 1);
				player.getPlayArea ().discardWeapons ();
				//TODO: advance quest
			} else {
				quest.removeParticipatingPlayer (player);
				//TODO: somehow finish removing a player from the quest. Does this require more work?
			}
		}
	}
}
