using System;
using UnityEngine;

[Serializable]
public class ProsperityThroughoutTheRealm : Events {

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
        action = () => {
			Action completeAction = () => {
				Logger.getInstance ().debug ("In ProsperityThroughoutTheRealm in DealCards(), about to RPC DealCardsNextPlayer");
				Debug.Log("In ProsperityThroughoutTheRealm in DealCards(), about to RPC DealCardsNextPlayer");
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
        playerToPrompt = board.getNextPlayer(playerToPrompt);
        if (playerToPrompt != originalPlayer) {
            DealCards();
        } else {
            board.nextTurn();
        }
    }
}
