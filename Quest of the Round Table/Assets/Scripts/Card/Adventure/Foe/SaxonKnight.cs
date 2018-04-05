using System;

[Serializable]
public class SaxonKnight : Foe {

	public static int frequency = 8;

	public SaxonKnight() : base("Saxon Knight", 15) {
		empoweredBattlePoints = 25;
	}
}
