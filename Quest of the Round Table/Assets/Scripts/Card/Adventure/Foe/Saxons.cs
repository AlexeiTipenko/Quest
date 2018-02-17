using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxons : Foe {

	public static int frequency = 5;

	public Saxons() : base("Saxons", 10) {
		empoweredBattlePoints = 20;
	}
}
