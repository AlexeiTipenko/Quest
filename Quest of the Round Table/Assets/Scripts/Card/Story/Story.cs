using System;

[Serializable]
public abstract class Story : Card {
	
	protected Story (string cardName) : base (cardName) {
		
	}

	public virtual void startBehaviour () {}

}


