using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProsperityThroughoutTheRealm : Event {

	public static int frequency = 1;

	public ProsperityThroughoutTheRealm () : base ("Prosperity Throughout the Realm") {

	}

	//Event description: All players may immediately draw 2 Adventure Cards. 
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Prosperity Throughout the Realm behaviour");

		List<Player> allPlayers = BoardManagerMediator.getInstance().getPlayers();

		foreach (Player player in allPlayers) {
			BoardManagerMediator.getInstance().dealCardsToPlayer (player, 2);
			Logger.getInstance ().trace ("Finished dealing 2 cards to player " + player.getName());
		}
		Logger.getInstance ().info ("Finished Prosperity Throughout the Realm behaviour");
	}
}
