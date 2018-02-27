using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pox : Event {

	public static int frequency = 1;

	public Pox () : base ("Pox") { }
		
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
        BoardManagerMediator.getInstance().NextPlayerTurn(currentPlayer);
	}
}
