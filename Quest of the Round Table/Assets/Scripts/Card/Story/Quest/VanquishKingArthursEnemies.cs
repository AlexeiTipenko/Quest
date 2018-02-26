using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanquishKingArthursEnemies : Quest {

	public static int frequency = 2;

	public VanquishKingArthursEnemies() : base ("Vanquish King Arthur's Enemies", 3) {
		Logger.getInstance ().info ("Initializing the Vanquish King Arthur's Enemies card");
	}

}
