using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChivalrousDeed : Event {

	public static int frequency = 1;

	public ChivalrousDeed() : base ("Chivalrous Deed") {

	}

	//Event description: Player with both lowest rank and least amount of shields, receives 3 shields.
	public override void startBehaviour() { 
		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		List<Player> lowestRankPlayers = new List<Player>();
		List<Player> lowestPlayers = new List<Player> ();

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

		//in the list of players with the lowest rank, make a list of players among those with the lowest # of shields
		foreach (Player player in lowestRankPlayers) {
			if (lowestPlayers.Count == 0) {
				lowestPlayers.Add (player);
			} else if (player.getNumShields () < lowestPlayers [0].getNumShields ()) {
				lowestPlayers.Clear ();
				lowestPlayers.Add (player);
			} else if (player.getNumShields () == lowestPlayers [0].getNumShields ()) {
				lowestPlayers.Add (player);
			}
		}

		//Award 3 shields
		foreach (Player player in lowestPlayers) {
			player.incrementShields (3);
		}
	}
}
