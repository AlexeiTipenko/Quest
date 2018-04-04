using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Card {

	protected Player owner;
	protected string cardName;
	public string cardImageName;

	protected Card(string cardName) {
        Logger.getInstance ().info("Creating card: " + cardName);
		this.cardName = cardName;
		this.owner = null;
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

	public void setCardImageName(string name) {
		cardImageName = name;
	}


	// public void discardCard() {}
	// etc

	public string toString() {
		return cardName + " (" + owner.getName() + ")";
	}
}
