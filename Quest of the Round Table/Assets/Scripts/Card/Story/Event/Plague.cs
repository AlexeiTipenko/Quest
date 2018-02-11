using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Event {

	public static int frequency = 1;

	public Plague () : base ("Plague") {

	}

	//Event description: Drawer loses 2 shields if possible.
	public override void startBehaviour() {
		GameObject boardManager = GameObject.Find("BoardManager");
		BoardManager boardScripts = boardManager.GetComponent<BoardManager> ();

		Player currentPlayer = boardScripts.getCurrentPlayer ();

		currentPlayer.decrementShields (2);
	}
}
