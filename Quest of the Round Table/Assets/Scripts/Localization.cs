using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization {

	private static BoardManagerMediator board;

	private static string sponsorQuestSelf = "NEW QUEST DRAWN\nWould you like to sponsor this quest?";
	private static string sponsorQuestOther = "NEW QUEST DRAWN\nAsking {player} to sponsor quest.";

	private static string sponsorTournamentSelf = "NEW TOURNAMENT DRAWN\nWould you like to sponsor this tournament?";
	private static string sponsorTournamentOther = "NEW QUEST DRAWN\nAsking {player} to sponsor tournament.";

	private static string prepareTournamentSelf = "PREPARE FOR BATTLE\nPrepare for the tournament using a combination of weapon, ally and amour cards.";
	private static string prepareTournamentOther = "PREPARING FOR BATTLE\n{player} is preparing for the tournament using a combination of weapon, ally and amour cards.";



	public static string sponsorQuest(Player player) {
		if (currentPlayerIsLocal(player)) {
			return sponsorQuestSelf;
		} else {
			return insertPlayer (sponsorQuestOther, player);
		}
	}

	public static string sponsorTournament(Player player) {
		if (currentPlayerIsLocal(player)) {
			return sponsorTournamentSelf;
		} else {
			return insertPlayer (sponsorTournamentOther, player);
		}
	}

	public static string prepareTournament(Player player) {
		if (currentPlayerIsLocal(player)) {
			return prepareTournamentSelf;
		} else {
			return insertPlayer (prepareTournamentOther, player);
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
