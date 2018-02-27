using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : Rank {

	public static int frequency = 4;

	public Squire() : base ("Squire", 5, 5, new Knight()) { }

}
