using System;

[Serializable]
public class Knight : Rank {

	public static int frequency = 4;

	public Knight() : base ("Knight", 10, 7, new ChampionKnight ()) { }

}
