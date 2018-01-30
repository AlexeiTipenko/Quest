using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtTintagel : Tournament
{
	public AtTintagel() : base () {}
	public AtTintagel(Player owner, string cardName) : base (owner, cardName) {
		bonusShields = 1;
	}
}

