using System;
using UnityEngine;

public class ProsperityThroughoutTheRealm : Event {

	public static int frequency = 1;
    BoardManagerMediator board;
    Player playerToPrompt, originalPlayer;
    Action action;

	public ProsperityThroughoutTheRealm () : base ("Prosperity Throughout the Realm") {
        board = BoardManagerMediator.getInstance();
	}

	//Event description: All players may immediately draw 2 Adventure Cards.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Prosperity Throughout the Realm behaviour");	
        playerToPrompt = board.getCurrentPlayer();
        originalPlayer = playerToPrompt;
        DealCards();
        Logger.getInstance().info("Finished Prosperity Throughout the Realm behaviour");
	}

    private void DealCards()
    {
        if (playerToPrompt.getHand().Count + 2 > 12) {
            action = () => {
                board.TransferFromHandToPlayArea(playerToPrompt);
                playerToPrompt.RemoveCardsResponse();
                DealCardsNextPlayer();
            };
            playerToPrompt.giveAction(action);
            BoardManagerMediator.getInstance().dealCardsToPlayer(playerToPrompt, 2);
        } else {
            BoardManagerMediator.getInstance().dealCardsToPlayer(playerToPrompt, 2);
            DealCardsNextPlayer();
        }
    }

    private void DealCardsNextPlayer() {
        playerToPrompt = board.getNextPlayer(playerToPrompt);
        if (playerToPrompt != originalPlayer) {
            DealCards();
        } else {
            board.nextTurn();
        }
    }
}
