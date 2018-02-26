using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {

	protected Player owner;
	protected string cardName;
	public string cardImageName;

	public Card(string cardName) {
		Logger.getInstance ().debug ("Creating card with name " + cardName);
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
