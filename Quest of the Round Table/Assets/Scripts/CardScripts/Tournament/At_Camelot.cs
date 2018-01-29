using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class At_Camelot : Tournament
{
	private static int bonusShields = 3;

	public At_Camelot() : base () {}
	public At_Camelot(Player owner, string cardName) : base (owner, cardName, bonusShields) {}
}


