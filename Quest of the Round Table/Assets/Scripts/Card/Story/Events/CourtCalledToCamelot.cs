using System.Collections.Generic;
using System;

[Serializable]
public class CourtCalledToCamelot : Events {

    BoardManagerMediator board;
    string description = "All Allies in play must be discarded.";
	public static int frequency = 2;

	public CourtCalledToCamelot () : base ("Court Called to Camelot") {
        board = BoardManagerMediator.getInstance();
    }

	//Event description: All Allies in play must be discarded.
	//TODO; Waiting to get reference to the play section for the player

	public override void startBehaviour() {
		Logger.getInstance ().info ("Started the Court Called to Camelot behaviour");
		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		foreach (Player player in allPlayers) {
			player.getPlayArea ().discardAllies ();
			Logger.getInstance ().debug ("Discarded all allies for " + player.getName());
		}
		Logger.getInstance ().info ("Finishing behaviour");

        Player currentPlayer = board.getCurrentPlayer();
        if (currentPlayer.GetType() != typeof(AIPlayer)) {
            board.SimpleAlert(currentPlayer, description);
        } else {
            board.nextTurn();
        }
	}
}
