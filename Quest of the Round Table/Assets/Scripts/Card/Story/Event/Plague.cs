using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Event {

	public static int frequency = 1;

	public Plague () : base ("Plague") { }

	//Event description: Drawer loses 2 shields if possible.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started Plague behaviour");
		Player currentPlayer = BoardManagerMediator.getInstance().getCurrentPlayer ();

		currentPlayer.decrementShields (2);
		Logger.getInstance().info("Finished Plague behaviour");
	}
}
