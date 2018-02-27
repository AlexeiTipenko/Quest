using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarHunt : Quest {

	public static int frequency = 2;

	public BoarHunt() : base ("Boar Hunt", 2) {

		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("Boar", true));
	}

}
