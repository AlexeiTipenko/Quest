using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Foe {

	public static int frequency = 4;

	public Boar() : base ("Boar", 5) {
		empoweredBattlePoints = 15;
	}
}
