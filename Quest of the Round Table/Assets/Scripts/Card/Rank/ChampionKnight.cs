using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionKnight : Rank {

	public static int frequency = 4;

	public ChampionKnight() : base ("Champion Knight", 20, 10, new KnightOfTheRoundTable ()) {

	}
}
