using System;

[Serializable]
public class GreenKnight : Foe {

	public static int frequency = 2;

	public GreenKnight() : base ("Green Knight", 25) {
		empoweredBattlePoints = 40;
	}
}
