using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {


	protected Player owner;
	protected string cardName;

	public Card(string cardName) {
		this.cardName = cardName;
	}


	//Getters
	public Player getOwner() {
		return this.owner;
	}

	public string getCardName() {
		return this.cardName;
	}

	//Setters
	public void setOwner(Player owner) {
		this.owner = owner;
	}


	// public void discardCard() {}
	// etc
}
