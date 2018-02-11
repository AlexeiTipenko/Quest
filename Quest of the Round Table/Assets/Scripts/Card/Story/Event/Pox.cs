using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pox : Event {

	public static int frequency = 1;

	public Pox () : base ("Pox") {

	}
		
	//Event description: All players except the player drawing this card lose 1 shield.
	public override void startBehaviour() {
		List<Player> allPlayers = BoardManagerData.getPlayers();

		Player currentPlayer = BoardManagerData.getCurrentPlayer ();

		foreach (Player player in allPlayers) {
			if (player != currentPlayer) {
				player.decrementShields (1);
			}
		}
	}
}
