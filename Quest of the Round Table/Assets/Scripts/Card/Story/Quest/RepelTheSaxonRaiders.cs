using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelTheSaxonRaiders : Quest {

	public static int frequency = 2;

	public RepelTheSaxonRaiders() : base ("Repel the Saxon Raiders", 2) {
		Logger.getInstance ().info ("Initializing the Repel the Saxon Raiders card");

		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("SaxonKnight", true));
		dominantFoes.Add(Type.GetType("Saxons", true));
	}
}
