using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOfTheGreenKnight : Quest {

	public static int frequency = 1;

	public TestOfTheGreenKnight() : base ("Test of the Green Knight", 4) {
		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("GreenKnight", true));
	}
}
