using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	private string name;
	private int numShields;
	private List<Card> hand;


	public Player(string name) {
		this.name = name;
		hand = new List<Card>();
	}

	public void dealCards(List<Card> cards) {
		foreach (Card card in cards) {
			card.setOwner (this);
			hand.Add (card);
		}

		if (hand.Count > 12) {
			//do something
		}
	}


	//Getters
	public string getName() {
		return this.name;
	}

	public int getNumShields() {
		return this.numShields;
	}

	public List<Card> getHand() {
		return this.hand;
	}


	//Setters
	private void setName(string name) {
		this.name = name;
	}

	private void incrementShields(int numShields) {
		this.numShields = numShields;
	}

	private void decrementShields(int numShields) {
		this.numShields = numShields;
	}

	public string toString() {
		string toString = this.name + "'s hand: ";

		foreach (Card card in hand) {
			toString += card.toString() + ", ";
		}
		toString = toString.Substring (0, toString.Length - 2);
		return toString;
	}
}


