using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueensFavor : Event {

	public static int frequency = 2;

	public QueensFavor () : base ("Queen's Favor") {

	}
		
	//Event description: The lowest ranked player(s) immediately receives 2 Adventure Cards.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Queen's Favor behaviour");

		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

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

		Logger.getInstance ().info ("Populated list of players with the lowest rank");

		//Award 2 Adventure cards
		foreach (Player player in lowestRankPlayers) {
			BoardManagerMediator.getInstance().dealCardsToPlayer (player, 2);
			Logger.getInstance ().trace ("Finished dealing 2 cards to player " + player.getName());
		}
		Logger.getInstance ().info ("Finished Queen's Favor behaviour");
	}
}
