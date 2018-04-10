using System;
using System.Collections.Generic;

[Serializable]
public class RescueTheFairMaiden : Quest {

	public static int frequency = 1;

	public RescueTheFairMaiden() : base ("Rescue the Fair Maiden", 3) {

		dominantFoes = new List<Type> ();
		dominantFoes.Add(Type.GetType("BlackKnight", true));
	}

}
