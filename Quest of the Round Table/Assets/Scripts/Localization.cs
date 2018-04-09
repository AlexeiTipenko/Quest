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

	public static string promptSponsorQuest(Player player) {
		return preparePrompt (player, promptSponsorQuestSelf, promptSponsorQuestOther);
	}

	public static string sponsorQuest(Player player, bool valid) {
		if (valid) {
			return preparePrompt (player, sponsorQuestSelf, sponsorQuestOther);
		}
		return preparePrompt (player, sponsorQuestInvalidSelf, sponsorQuestOther);
	}

	private static string preparePrompt(Player player, string selfText, string otherText) {
		if (currentPlayerIsLocal (player)) {
			return selfText;
		}
		return insertPlayer(otherText, player);
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
