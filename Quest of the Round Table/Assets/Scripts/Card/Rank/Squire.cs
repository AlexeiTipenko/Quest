using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : Rank {
	
	public Squire() : base ("Squire", 5) {
		nextRank = new Knight ();
	}

}
