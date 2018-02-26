using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtOrkney : Tournament {

	public static int frequency = 1;

	public AtOrkney() : base ("At Orkney", 2) {
		Logger.getInstance ().info ("Initialized the At Orkney card");
	}
}


