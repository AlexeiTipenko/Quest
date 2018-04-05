using System;
using UnityEngine;

[Serializable]
public class KnightOfTheRoundTable : Rank {

	public static int frequency = 1;

	public KnightOfTheRoundTable() : base ("Knight of the Round Table", 1000000000, 10000000,null) { 
        Debug.Log("Fuck you if you're crashing here, fuck you Knight of round table");
    }
}
