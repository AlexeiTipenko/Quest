using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlayTheDragon : Quest {

	public static int frequency = 1;

	public SlayTheDragon() : base ("Slay the Dragon", 3) {
		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("Dragon", true));
	}

}
