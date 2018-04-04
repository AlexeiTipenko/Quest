using System;

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
				if (board.IsOnlineGame()) {
					board.getPhotonView().RPC("DealCardsNextPlayer", PhotonTargets.Others);
				}				
				DealCardsNextPlayer();
			};
            playerToPrompt.DiscardCards(action, completeAction);
        };

        playerToPrompt.DrawCards(2, action);
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
