using System;

[Serializable]
public class Thieves : Foe {

	public static int frequency = 8;

	public Thieves() : base ("Thieves", 5) {
		
	}
}
