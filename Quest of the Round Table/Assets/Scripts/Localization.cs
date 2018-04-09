using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization {

	private static BoardManagerMediator board;

	private static string promptSponsorQuestSelf = "NEW QUEST DRAWN\nWould you like to sponsor this quest?";
	private static string promptSponsorQuestOther = "NEW QUEST DRAWN\nAsking {player} to sponsor quest.";

	private static string sponsorQuestSelf = "PREPARE YOUR QUEST\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain(unique) weapons\n- Battle points must increase between stages";
	private static string sponsorQuestInvalidSelf = "INVALID QUEST SELECTIONS.\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages";
	private static string sponsorQuestOther = "QUEST BEING PREPARED\n{player} is preparing to sponsor the quest.";

	private static string sponsorTournamentSelf = "NEW TOURNAMENT DRAWN\nWould you like to sponsor this tournament?";
	private static string sponsorTournamentOther = "NEW QUEST DRAWN\nAsking {player} to sponsor tournament.";

	private static string prepareTournamentSelf = "PREPARE FOR BATTLE\nPrepare for the tournament using a combination of weapon, ally and amour cards.";
	private static string prepareTournamentOther = "PREPARING FOR BATTLE\n{player} is preparing for the tournament using a combination of weapon, ally and amour cards.";

	public static string PromptSponsorQuest(Player player) {
		return PreparePrompt (player, promptSponsorQuestSelf, promptSponsorQuestOther);
	}

	public static string SponsorQuest(Player player, bool valid) {
		if (valid) {
			return PreparePrompt (player, sponsorQuestSelf, sponsorQuestOther);
		}
		return PreparePrompt (player, sponsorQuestInvalidSelf, sponsorQuestOther);
	}

	public static string PromptEnterTournament(Player player) {
		return PreparePrompt (player, sponsorTournamentSelf, sponsorTournamentOther);
	}

	public static string PrepareTournament(Player player) {
		return PreparePrompt (player, prepareTournamentSelf, prepareTournamentOther);
	}

	private static string PreparePrompt(Player player, string selfText, string otherText) {
		if (CurrentPlayerIsLocal (player)) {
			return selfText;
		}
		return InsertPlayer(otherText, player);
	}

	private static bool CurrentPlayerIsLocal(Player player) {
		board = BoardManagerMediator.getInstance ();
		List<Player> players = board.getPlayers ();
		int playerTurn = players.IndexOf (player) + 1;
		return (playerTurn == PhotonNetwork.player.ID);
	}

	private static string InsertPlayer(string text, Player player) {
		return text.Replace ("{player}", player.getName ());
	}
}
