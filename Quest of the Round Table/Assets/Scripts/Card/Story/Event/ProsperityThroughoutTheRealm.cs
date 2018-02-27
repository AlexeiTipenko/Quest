using System.Collections.Generic;
using System;

public class ProsperityThroughoutTheRealm : Event {

	public static int frequency = 1;
    private BoardManagerMediator board;
    private Player playerToPrompt;
    Action action;

	public ProsperityThroughoutTheRealm () : base ("Prosperity Throughout the Realm") {
        board = BoardManagerMediator.getInstance();
	}

	//Event description: All players may immediately draw 2 Adventure Cards. 
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Prosperity Throughout the Realm behaviour");	
        playerToPrompt = board.getCurrentPlayer();
        DealCards();
        Logger.getInstance().info("Finished Prosperity Throughout the Realm behaviour");
	}

    private void DealCards() {
        
        action = () => {

            if (BoardManagerMediator.getInstance().GetCardsNumHandArea(playerToPrompt) > 12)
                board.PromptCardRemoveSelection(playerToPrompt, action);

            else
            {
                board.TransferFromHandToPlayArea(playerToPrompt);
                playerToPrompt.RemoveCardsResponse();

                playerToPrompt = board.getNextPlayer(playerToPrompt);

                if (playerToPrompt != board.getCurrentPlayer())
                    DealCards();
                
                else
                    board.nextTurn();
            }       
        };

        playerToPrompt.giveAction(action);
        BoardManagerMediator.getInstance().dealCardsToPlayer(playerToPrompt, 2);
    }
}
