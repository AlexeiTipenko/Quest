using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Adventure {

	protected int minBidValue;

	public Test() {
		minBidValue = 3;
	}

	public Test(int minBidValue) {
		this.minBidValue = minBidValue;
	}

	public int getMinBidValue() {
		return minBidValue;
	}
}
