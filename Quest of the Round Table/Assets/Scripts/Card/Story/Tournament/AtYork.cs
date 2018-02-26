using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtYork : Tournament {

	public static int frequency = 1;
	
	public AtYork() : base ("At York", 0) {
		Logger.getInstance ().info ("Initialized the At York card");
	}
}

