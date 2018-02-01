using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Test : Adventure {

	protected int minBidValue;

	public Test(string cardName, int minBidValue) : base (cardName) {
		this.minBidValue = minBidValue;
	}

	public int getMinBidValue() {
		return minBidValue;
	}
}
