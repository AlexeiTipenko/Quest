using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Story : Card {
	
	public Story (string cardName) : base (cardName) {
		
	}

	public override void startBehaviour () {
		Debug.Log ("Starting card behaviour for card: " + cardName);
	}

}


