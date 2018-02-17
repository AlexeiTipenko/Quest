using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKnight : Foe {

	public static int frequency = 6;

	public EvilKnight() : base ("Evil Knight", 20) {
		empoweredBattlePoints = 30;
	}
}
