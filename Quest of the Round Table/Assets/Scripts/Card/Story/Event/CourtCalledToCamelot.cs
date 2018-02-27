using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtCalledToCamelot : Event {
	
	public static int frequency = 2;

	public CourtCalledToCamelot () : base ("Court Called to Camelot") { }

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
	}
}
