using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProsperityThroughoutTheRealm : Event {

	public static int frequency = 1;

	public ProsperityThroughoutTheRealm () : base ("Prosperity Throughout the Realm") {

	}

	//Event description: All players may immediately draw 2 Adventure Cards. 
	public override void startBehaviour() {
		List<Player> allPlayers = BoardManagerData.getPlayers();

		foreach (Player player in allPlayers) {
			BoardManagerData.dealCardsToPlayer (player, 2);
		}
	}
}
