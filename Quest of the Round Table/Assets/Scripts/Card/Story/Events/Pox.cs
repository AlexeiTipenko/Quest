using System.Collections.Generic;

public class Pox : Events {

    BoardManagerMediator board;
    string description = "All players except the player drawing this card lose 1 shield.";
	public static int frequency = 1;

	public Pox () : base ("Pox") {
        board = BoardManagerMediator.getInstance();
    }
		
	//Event description: All players except the player drawing this card lose 1 shield.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Starting Pox behaviour");
		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		Player currentPlayer = BoardManagerMediator.getInstance().getCurrentPlayer ();

		foreach (Player player in allPlayers) {
			if (player != currentPlayer) {
				player.decrementShields (1);
			}
			Logger.getInstance ().trace ("Finished decrementing shields for player " + player.getName());
		}
		Logger.getInstance ().info ("Finished Pox behaviour");

        if (currentPlayer.GetType() != typeof(AIPlayer)) {
            board.SimpleAlert(currentPlayer, description);
        } else {
            board.nextTurn();
        }
	}
}
