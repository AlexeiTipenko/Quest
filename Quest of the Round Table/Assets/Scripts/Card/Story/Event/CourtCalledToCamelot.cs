using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtCalledToCamelot : Event {
	
	public static int frequency = 2;

	public CourtCalledToCamelot () : base ("Court Called to Camelot") {

	}
		
	//Event description: All Allies in play must be discarded.
	//TODO; Waiting to get reference to the play section for the player
	public override void startBehaviour() {
		GameObject boardManager = GameObject.Find("BoardManager");
		BoardManager boardScripts = boardManager.GetComponent<BoardManager> ();
		List<Player> allPlayers = boardScripts.getPlayers();

		foreach (Player player in allPlayers) {
			
		}
	}
}
