using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization {

	private static BoardManagerMediator board;

	private static string promptSponsorQuestSelf = "NEW QUEST DRAWN\nWould you like to sponsor this quest?";
	private static string promptSponsorQuestOther = "NEW QUEST DRAWN\nAsking {player} to sponsor this quest.";

	private static string sponsorQuestSelf = "PREPARE YOUR QUEST\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain(unique) weapons\n- Battle points must increase between stages";
	private static string sponsorQuestInvalidSelf = "INVALID QUEST SELECTIONS.\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages";
	private static string sponsorQuestOther = "QUEST BEING PREPARED\n{player} is preparing to sponsor the quest.";

	private static string promptAcceptQuestSelf = "NEW QUEST DRAWN\nWould you like to participate in this quest?";
	private static string promptAcceptQuestOther = "NEW QUEST DRAWN\nAsking {player} to participate in this quest.";

	private static string questStage = "QUEST STAGE {number}\n";

	private static string promptFoeSelf = "You are facing a foe. You may place any number of cards, or drop out.";
	private static string promptFoeOther = "{player} is preparing to take action against the foe for this stage.";

	private static string promptTestSelf = "The current stage is a test, with a minimum bid of: {number}. Do you wish to bid?";
	private static string promptTestOther = "{player} is bidding on the current test.";

	private static string promptDiscardTestSelf = "You are the winner of the test, and you must discard/play a total of {number} bid points.";
	private static string promptDiscardTestOther = "{player} won the test, and is currently discarding/playing the cards they bid.";

	private static string promptEnterTournamentSelf = "NEW TOURNAMENT DRAWN\nWould you like to enter this tournament?";
	private static string promptEnterTournamentOther = "NEW TOURNAMENT DRAWN\nAsking {player} to enter this tournament.";

	private static string prepareTournamentSelf = "PREPARE FOR BATTLE\nPrepare for the tournament using a combination of weapon, ally and amour cards.";
	private static string prepareTournamentOther = "PREPARING FOR BATTLE\n{player} is preparing for the tournament.";

	private static string promptToDiscardWeaponSelf = "Please discard 1 weapon.";
	private static string promptToDiscardWeaponOther = "Prompting {player} to discard 1 weapon.";

	private static string promptToDiscardFoesSelf = "Please discard {number} foes.";
	private static string promptToDiscardFoesOther = "Prompting {player} to discard {number} foes.";
		
	private static string playerPassedSelf = "You passed!";
	private static string playerPassedOther = "{player} passed.";
	private static string playerEliminatedSelf = "You were eliminated...";
	private static string playerEliminatedOther = "{player} was eliminated.";

	private static string promptCardRemoveSelectionSelf = "TOO MANY CARDS\nPlease discard (or play) cards until you have at most 12.";
	private static string promptCardRemoveSelectionOther = "TOO MANY CARDS\nPrompting {player} to discard (or play) cards until they have at most 12.";

	private static string playerWonSelf = "You won the tournament!";
	private static string playerWonOther = "{player} won the tournament.";

	private static string playerLostSelf = "You lost the tournament...";
	private static string playerLostOther = "{player} lost the tournament.";

	public static string PromptSponsorQuest(Player player) {
		return PreparePrompt (player, promptSponsorQuestSelf, promptSponsorQuestOther);
	}

	public static string SponsorQuest(Player player, bool valid) {
		if (valid) {
			return PreparePrompt (player, sponsorQuestSelf, sponsorQuestOther);
		}
		return PreparePrompt (player, sponsorQuestInvalidSelf, sponsorQuestOther);
	}

	public static string PromptAcceptQuest(Player player) {
		return PreparePrompt (player, promptAcceptQuestSelf, promptAcceptQuestOther);
	}

	public static string PromptFoe(Player player, int stageNum) {
		return PreparePrompt (player, QuestStage(stageNum) + promptFoeSelf, QuestStage(stageNum) + promptFoeOther);
	}

	public static string PromptTest(Player player, int stageNum, int minimumBid) {
		return PreparePrompt (player, QuestStage(stageNum) + InsertNumber(promptTestSelf, minimumBid), QuestStage(stageNum) + promptTestOther);
	}

	public static string PromptDiscardTest(Player player, int stageNum, int currentBid) {
		return PreparePrompt (player, QuestStage (stageNum) + InsertNumber (promptDiscardTestSelf, currentBid), QuestStage (stageNum) + promptDiscardTestOther);
	}

	public static string PromptEnterTournament(Player player) {
		return PreparePrompt (player, promptEnterTournamentSelf, promptEnterTournamentOther);
	}

	public static string PrepareTournament(Player player) {
		return PreparePrompt (player, prepareTournamentSelf, prepareTournamentOther);
	}

	public static string PromptToDiscardWeapon(Player player) {
		return PreparePrompt (player, promptToDiscardWeaponSelf, promptToDiscardWeaponOther);
	}

	public static string PromptToDiscardFoes(Player player, int numFoes) {
		return PreparePrompt (player, InsertNumber (promptToDiscardFoesSelf, numFoes), InsertNumber (promptToDiscardFoesOther, numFoes));
	}

	public static string DisplayStageResults(Player player, bool playerEliminated) {
		if (playerEliminated) {
			return PreparePrompt (player, playerEliminatedSelf, playerEliminatedOther);
		}
		return PreparePrompt (player, playerPassedSelf, playerPassedOther);
	}

	public static string PromptCardRemoveSelection(Player player) {
		return PreparePrompt (player, promptCardRemoveSelectionSelf, promptCardRemoveSelectionOther);
	}

	public static string DisplayTournamentResults(Player player, bool playerLost) {
		if (playerLost) {
			return PreparePrompt (player, playerLostSelf, playerLostOther);
		}
		return PreparePrompt (player, playerWonSelf, playerWonOther);
	}

	private static string PreparePrompt(Player player, string selfText, string otherText) {
		if (CurrentPlayerIsLocal (player)) {
			return selfText;
		}
		return InsertPlayer(otherText, player);
	}

	private static bool CurrentPlayerIsLocal(Player player) {
		board = BoardManagerMediator.getInstance ();
		if (board.IsOnlineGame ()) {
			List<Player> players = board.getPlayers ();
			int playerTurn = players.IndexOf (player) + 1;
			return (playerTurn == PhotonNetwork.player.ID);
		}
		return true;
	}

	private static string QuestStage(int stageNum) {
		return InsertNumber (questStage, stageNum);
	}

	private static string InsertPlayer(string text, Player player) {
		return text.Replace ("{player}", player.getName ());
	}

	private static string InsertNumber(string text, int number) {
		return text.Replace ("{number}", number.ToString());
	}
}
