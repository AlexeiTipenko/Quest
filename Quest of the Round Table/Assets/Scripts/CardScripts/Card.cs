using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {


	protected Player owner;
	protected string cardName;


	public Card(){ }

	public Card(Player owner, string cardName) {
		this.owner = owner;
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
	private void setOwner(Player owner){
		this.owner = owner;
	}


	// public void discardCard() {}
	// etc
}
