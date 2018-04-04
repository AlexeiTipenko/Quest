using System;

[Serializable]
public class Boar : Foe {

	public static int frequency = 4;

	public Boar() : base ("Boar", 5) {
		empoweredBattlePoints = 15;
	}
}
