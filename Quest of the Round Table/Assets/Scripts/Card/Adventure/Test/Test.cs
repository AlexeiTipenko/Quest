using System;

[Serializable]
public abstract class Test : Adventure {

	protected int minBidValue;

	public Test(string cardName, int minBidValue) : base (cardName) {
		this.minBidValue = minBidValue;
	}

	public virtual int getMinBidValue() {
		return minBidValue;
	}
}
