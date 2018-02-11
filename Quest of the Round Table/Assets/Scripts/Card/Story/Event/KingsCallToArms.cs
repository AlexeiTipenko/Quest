using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsCallToArms : Event {

	public static int frequency = 1;

	public KingsCallToArms () : base ("King's Call to Arms") {

	}
		
	//Event description: The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded.
	public override void startBehaviour() {

	}
}
