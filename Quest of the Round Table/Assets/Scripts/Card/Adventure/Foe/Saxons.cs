using System;

[Serializable]
public class Saxons : Foe {

	public static int frequency = 5;

	public Saxons() : base("Saxons", 10) {
		empoweredBattlePoints = 20;
	}
}
