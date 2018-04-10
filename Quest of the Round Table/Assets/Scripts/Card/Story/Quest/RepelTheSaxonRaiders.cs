using System;
using System.Collections.Generic;

[Serializable]
public class RepelTheSaxonRaiders : Quest {

	public static int frequency = 2;

	public RepelTheSaxonRaiders() : base ("Repel the Saxon Raiders", 2) {

		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("SaxonKnight", true));
		dominantFoes.Add(Type.GetType("Saxons", true));
	}
}
