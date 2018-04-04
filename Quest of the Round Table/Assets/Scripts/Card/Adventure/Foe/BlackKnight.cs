using System;

[Serializable]
public class BlackKnight : Foe {

	public static int frequency = 3;

	public BlackKnight() : base("Black Knight", 25) {
		empoweredBattlePoints = 35;
	}

}
