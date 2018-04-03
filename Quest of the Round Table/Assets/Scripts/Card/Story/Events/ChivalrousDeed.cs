using System.Collections.Generic;

public class ChivalrousDeed : Events {

    BoardManagerMediator board;
    string description = "Player with both lowest rank and least amount of shields, receives 3 shields.";
	public static int frequency = 1;

	public ChivalrousDeed() : base ("Chivalrous Deed") {
        board = BoardManagerMediator.getInstance();
    }

	//Event description: Player with both lowest rank and least amount of shields, receives 3 shields.
	public override void startBehaviour() { 
		Logger.getInstance ().info ("Started the Chivalrous Deed behaviour");

		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		List<Player> lowestRankPlayers = new List<Player>();
		List<Player> lowestPlayers = new List<Player> ();

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

		Logger.getInstance ().debug ("Finished getting list of players with lowest rank");
		//in the list of players with the lowest rank, make a list of players among those with the lowest # of shields
		foreach (Player player in lowestRankPlayers) {
			if (lowestPlayers.Count == 0) {
				lowestPlayers.Add (player);
			} else if (player.getNumShields () < lowestPlayers [0].getNumShields ()) {
				lowestPlayers.Clear ();
				lowestPlayers.Add (player);
			} else if (player.getNumShields () == lowestPlayers [0].getNumShields ()) {
				lowestPlayers.Add (player);
			}
		}
		Logger.getInstance ().debug ("Finished getting list of players with lowest # shields");

		//Award 3 shields
		foreach (Player player in lowestPlayers) {
			player.incrementShields (3);
			Logger.getInstance ().info ("Awarded 3 shields to " + player.getName());
		}
		Logger.getInstance ().info ("Awarded 3 shields to lowest ranked + lowest shields players, finishing behaviour");

        Player currentPlayer = board.getCurrentPlayer();
        if (currentPlayer.GetType() != typeof(AIPlayer)) {
            board.SimpleAlert(currentPlayer, description);
        } else {
            board.nextTurn();
        }
	}
}
