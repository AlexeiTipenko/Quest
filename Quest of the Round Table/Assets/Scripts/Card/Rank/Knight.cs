using System;
using UnityEngine;

[Serializable]
public class Knight : Rank {

	public static int frequency = 4;

	public Knight() : base ("Knight", 10, 7, new ChampionKnight ()) {
        Debug.Log("Fuck you if you're crashing here, fuck you Knight");
    }

}
