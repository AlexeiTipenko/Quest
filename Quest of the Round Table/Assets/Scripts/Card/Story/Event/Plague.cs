public class Plague : Event {

    BoardManagerMediator board;
    string description = "Drawer loses 2 shields if possible.";
	public static int frequency = 1;

	public Plague () : base ("Plague") {
        board = BoardManagerMediator.getInstance();
    }

	//Event description: Drawer loses 2 shields if possible.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Plague behaviour");
		Player currentPlayer = BoardManagerMediator.getInstance().getCurrentPlayer ();

		currentPlayer.decrementShields (2);
		Logger.getInstance().info("Finished Plague behaviour");

        if (currentPlayer.GetType() != typeof(AIPlayer)) {
            board.SimpleAlert(currentPlayer, description);
        } else {
            board.nextTurn();
        }
	}
}
