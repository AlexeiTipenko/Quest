using System;

[Serializable]
public class Dragon : Foe {

	public static int frequency = 1;

	public Dragon() : base ("Dragon", 50) {
		empoweredBattlePoints = 70;
	}
}
