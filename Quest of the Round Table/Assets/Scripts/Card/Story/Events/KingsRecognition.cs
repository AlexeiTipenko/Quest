using System;

[Serializable]
public class KingsRecognition : Events {

    BoardManagerMediator board;
    string description = "The next player(s) to complete a Quest will receive 2 extra shields.";
	public static int frequency = 2;

	public KingsRecognition () : base ("King's Recognition") {
        board = BoardManagerMediator.getInstance();
    }
		
	//Event description: The next player(s) to complete a Quest will receive 2 extra shields.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started King's Recognition behaviour");
		Quest.KingsRecognitionActive = true;
		Logger.getInstance ().info ("Finished King's Recognition behaviour");

        Player currentPlayer = BoardManagerMediator.getInstance().getCurrentPlayer();
        if (currentPlayer.GetType() != typeof(AIPlayer)) {
            board.SimpleAlert(currentPlayer, description);
        } else {
            board.nextTurn();
        }
	}
}
