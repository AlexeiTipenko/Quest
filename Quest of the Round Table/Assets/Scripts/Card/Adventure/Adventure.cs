using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure : Card {

	protected int battlePoints, bidPoints;

	public Adventure() { }
		

	public int getBattlePoints() {
		return battlePoints;
	}

	public int getBidPoints() {
		return bidPoints;
	}
}
