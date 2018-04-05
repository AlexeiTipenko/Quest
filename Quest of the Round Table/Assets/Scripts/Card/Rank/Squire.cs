using System;
using UnityEngine;

[Serializable]
public class Squire : Rank {

	public static int frequency = 4;

	public Squire() : base ("Squire", 5, 5, new Knight()) {
        Debug.Log("Fuck you if you're crashing here, fuck you squire");
    }

}
