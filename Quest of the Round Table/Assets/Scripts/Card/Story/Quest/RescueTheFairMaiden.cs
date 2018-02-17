using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueTheFairMaiden : Quest {

	public static int frequency = 1;

	public RescueTheFairMaiden() : base ("Rescue the Fair Maiden", 3) {
		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("BlackKnight", true));
	}

}
