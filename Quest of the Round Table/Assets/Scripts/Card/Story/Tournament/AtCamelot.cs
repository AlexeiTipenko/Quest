using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtCamelot : Tournament
{

	public AtCamelot() : base () {}
	public AtCamelot(Player owner, string cardName) : base (owner, cardName) {
		bonusShields = 3;
	}

}


