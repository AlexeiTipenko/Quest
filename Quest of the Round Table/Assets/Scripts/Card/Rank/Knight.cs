using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Rank {

	public static int frequency = 4;

	public Knight() : base ("Knight", 10, 7, new ChampionKnight ()) {
        Logger.getInstance().debug("A Knight instance has been initialized");
	}

}
