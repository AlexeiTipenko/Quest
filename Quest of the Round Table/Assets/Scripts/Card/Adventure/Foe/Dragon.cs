using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Foe {

	public static int frequency = 1;

	public Dragon() : base ("Dragon", 50) {
		empoweredBattlePoints = 70;
	}
}
