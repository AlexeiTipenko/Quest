using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForTheHolyGrail : Quest {

	public static int frequency = 1;

	public SearchForTheHolyGrail() : base ("Search for the Holy Grail", 5) {

		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("BlackKnight", true));
		dominantFoes.Add(Type.GetType("Boar", true));
		dominantFoes.Add(Type.GetType("Dragon", true));
		dominantFoes.Add(Type.GetType("EvilKnight", true));
		dominantFoes.Add(Type.GetType("Giant", true));
		dominantFoes.Add(Type.GetType("GreenKnight", true));
		dominantFoes.Add(Type.GetType("Mordred", true));
		dominantFoes.Add(Type.GetType("RobberKnight", true));
		dominantFoes.Add(Type.GetType("SaxonKnight", true));
		dominantFoes.Add(Type.GetType("Saxons", true));
		dominantFoes.Add(Type.GetType("Thieves", true));
	}

}
