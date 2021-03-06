using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
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
			Action completeAction = () => {
				Logger.getInstance ().debug ("In QueensFavor DealCards(), about to RPC DealCardsNextPlayer");
				Debug.Log("In QueensFavor DealCards(), about to RPC DealCardsNextPlayer");
				if (board.IsOnlineGame() && playerToPrompt.discarded) {
					playerToPrompt.toggleDiscarded(false);
					board.getPhotonView().RPC("DealCardsNextPlayer", PhotonTargets.Others);
				}
				DealCardsNextPlayer();
			};
            if (playerToPrompt.GetHand().Count > 12) {
                playerToPrompt.DiscardCards(action, completeAction);
            }
            else {
                DealCardsNextPlayer();
            }
        };

        playerToPrompt.DrawCards(2, action);
    }

    public void DealCardsNextPlayer() {
        playerToPrompt = GetNextPlayer(playerToPrompt);
        if (playerToPrompt != originalPlayer)
        {
			Debug.Log ("dealing cards in DealCardsNextPlayer");
            DealCards();
        }
        else
        {
			Debug.Log ("next turn in DealCardsNextPlayer");
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
