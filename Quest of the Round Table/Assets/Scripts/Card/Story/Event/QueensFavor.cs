using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueensFavor : Event {

	public static int frequency = 2;

	public QueensFavor () : base ("Queen's Favor") {

	}
		
	//Event description: The lowest ranked player(s) immediately receives 2 Adventure Cards.
	public override void startBehaviour() {
		List<Player> allPlayers = BoardManagerData.getPlayers();

		List<Player> lowestRankPlayers = new List<Player>();

		//populate a list of players with the lowest rank
		foreach (Player player in allPlayers) {
			if (lowestRankPlayers.Count == 0) {
				lowestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() < lowestRankPlayers [0].getRank ().getBattlePoints()) {
				lowestRankPlayers.Clear ();
				lowestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() == lowestRankPlayers [0].getRank ().getBattlePoints()) {
				lowestRankPlayers.Add (player);
			}
		}

		//Award 2 Adventure cards
		foreach (Player player in lowestRankPlayers) {
			BoardManagerData.dealCardsToPlayer (player, 2);
		}
	}
}
