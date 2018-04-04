using System;

[Serializable]
public abstract class Events : Story {

	public Events (string cardName) : base (cardName) {
		
	}

	public override void startBehaviour () {}

}
