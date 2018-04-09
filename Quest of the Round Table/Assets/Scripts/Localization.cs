using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization {

	private static BoardManagerMediator board;

	private static string sponsorQuestSelf = "NEW QUEST DRAWN\nWould you like to sponsor this quest?";
	private static string sponsorQuestOther = "NEW QUEST DRAWN\nAsking {player} to sponsor quest.";

	public static string sponsorQuest(Player player) {
		if (currentPlayerIsLocal(player)) {
			return sponsorQuestSelf;
		} else {
			return insertPlayer (sponsorQuestOther, player);
		}
	}

	private static bool currentPlayerIsLocal(Player player) {
		board = BoardManagerMediator.getInstance ();
		List<Player> players = board.getPlayers ();
		int playerTurn = players.IndexOf (player) + 1;
		return (playerTurn == PhotonNetwork.player.ID);
	}

	private static string insertPlayer(string text, Player player) {
		return text.Replace ("{player}", player.getName ());
	}
}
