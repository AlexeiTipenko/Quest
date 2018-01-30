using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtOrkney : Tournament
{
	public AtOrkney() : base () {}
	public AtOrkney(Player owner, string cardName) : base (owner, cardName) {
		bonusShields = 2;
	}
}


