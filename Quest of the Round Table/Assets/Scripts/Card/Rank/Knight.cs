using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Rank {
	
	public Knight() : base ("Knight", 10) {
		nextRank = new ChampionKnight ();
	}

}
