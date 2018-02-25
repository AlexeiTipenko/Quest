using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsCallToArms : Event {

	public static int frequency = 1;
	private List<Player> highestRankPlayers; 
	public Player currentPlayer;

	public KingsCallToArms () : base ("King's Call to Arms") {

	}

	//Event description: The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded.
	public override void startBehaviour() {
		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		//Find highest ranked player(s)
		highestRankPlayers = new List<Player>();

		//populate a list of players with the lowest rank
		foreach (Player player in allPlayers) {
			if (highestRankPlayers.Count == 0) {
				highestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() > highestRankPlayers [0].getRank ().getBattlePoints()) {
				highestRankPlayers.Clear ();
				highestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() == highestRankPlayers [0].getRank ().getBattlePoints()) {
				highestRankPlayers.Add (player);
			}
		}

		if (highestRankPlayers.Count != 0) { // For safety
			currentPlayer = highestRankPlayers [0];
			PromptEventAction ();
		}
	}

	public void PromptEventAction() {
		int numFoeCards = getNumFoeCards ();

		if (hasWeapons ()) {
			Debug.Log ("Player " + currentPlayer.getName () + " to discard a weapon");
			BoardManagerMediator.getInstance().PromptToDiscardWeapon (currentPlayer);
		} 
		else if (numFoeCards > 0) {
			Debug.Log ("Player " + currentPlayer.getName () + " to discard " + numFoeCards + " foes.");
			BoardManagerMediator.getInstance().PromptToDiscardFoes (currentPlayer, numFoeCards);
		}
		else {
			//call same function on next player
			currentPlayer = getNextPlayer(currentPlayer);
			if (currentPlayer != null) {
				Debug.Log ("Prompting event action for next player");
				PromptEventAction ();
			}
		}
		Debug.Log ("Finished King's Call To Arms.");
	}

	public Player getNextPlayer(Player previousPlayer) {
		int index = highestRankPlayers.IndexOf (previousPlayer);
		if (index != -1) {
			return highestRankPlayers [(index + 1) % highestRankPlayers.Count];
		}
		return null;
	}
		
	public void PlayerFinishedResponse() {
		//call action on next player
		currentPlayer = getNextPlayer (currentPlayer);
		PromptEventAction ();
	}

	private int getNumFoeCards() {
		int numFoeCards = 0;
		foreach (Card card in currentPlayer.getHand()) {
			if (card.GetType ().IsSubclassOf (typeof(Foe))) {
				numFoeCards++;
				if (numFoeCards == 2) {
					return numFoeCards;
				}
			}
		}
		return numFoeCards;
	}

	public bool hasWeapons() {
		foreach (Card card in currentPlayer.getHand()) {
			if (card.GetType().IsSubclassOf (typeof(Weapon))) {
				return true;
			}
		}
		return false;
	}
}
