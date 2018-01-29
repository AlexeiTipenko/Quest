using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Adventure {

	private int minBidValue;

	public Test() {
		minBidValue = 3;
	}

	public int getMinBidValue() {
		return minBidValue;
	}
}
