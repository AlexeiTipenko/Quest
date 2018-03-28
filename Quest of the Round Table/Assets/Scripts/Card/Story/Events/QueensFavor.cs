using System.Collections.Generic;
using System;

public class QueensFavor : Events {

	public static int frequency = 2;
    private BoardManagerMediator board;
    private Player playerToPrompt, originalPlayer;
    List<Player> lowestRankPlayers;
    Action action;

	public QueensFavor () : base ("Queen's Favor") {
        board = BoardManagerMediator.getInstance();
	}
		
	//Event description: The lowest ranked player(s) immediately receives 2 Adventure Cards.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Queen's Favor behaviour");

		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		lowestRankPlayers = new List<Player>();

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

        playerToPrompt = board.getCurrentPlayer();
        while (!lowestRankPlayers.Contains(playerToPrompt)) {
            playerToPrompt = board.getNextPlayer(playerToPrompt);
        }
        originalPlayer = playerToPrompt;
        DealCards();
        Logger.getInstance ().info ("Finished Queen's Favor behaviour");
	}

    private void DealCards()
    {
        action = () => {
            Action completeAction = DealCardsNextPlayer;
            playerToPrompt.DiscardCards(action, completeAction);
        };

        playerToPrompt.DrawCards(2, action);
    }

    private void DealCardsNextPlayer() {
        playerToPrompt = GetNextPlayer(playerToPrompt);
        if (playerToPrompt != originalPlayer)
        {
            DealCards();
        }
        else
        {
            board.nextTurn();
        }
    }

    public Player GetNextPlayer(Player previousPlayer)
    {
        int index = lowestRankPlayers.IndexOf(previousPlayer);
        if (index != -1)
        {
            return lowestRankPlayers[(index + 1) % lowestRankPlayers.Count];
        }
        return null;
    }
}
