using System;
using UnityEngine;

[Serializable]
public class ChampionKnight : Rank {

	public static int frequency = 4;

	public ChampionKnight() : base ("Champion Knight", 20, 10, new KnightOfTheRoundTable ()) {
        Debug.Log("Fuck you if you're crashing here, fuck you championKnight");
    }
}
