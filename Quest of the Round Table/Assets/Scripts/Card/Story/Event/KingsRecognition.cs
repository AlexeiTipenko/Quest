using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsRecognition : Event {

	public static int frequency = 2;

	public KingsRecognition () : base ("King's Recognition") { }
		
	//Event description: The next player(s) to complete a Quest will receive 2 extra shields.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started King's Recognition behaviour");
		Quest.KingsRecognitionActive = true;
		Logger.getInstance ().info ("Finished King's Recognition behaviour");
	}
}
