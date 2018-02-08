﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	private string name;
	private int numShields;
	private Rank rank;
	private List<Card> hand;
	private bool isAI;


	public Player(string name, bool isAI) {
		this.name = name;
		this.isAI = isAI;
		rank = new Squire ();
		numShields = 0;
		hand = new List<Card>();
	}

	public void dealCards(List<Card> cards) {
		foreach (Card card in cards) {
			card.setOwner (this);
			hand.Add (card);
		}

		if (hand.Count > 12) {
			//prompt player to discard cards
		}
	}

	private void checkForRankUp() {
		if (numShields >= rank.getShieldsToProgress()) {
			rank = rank.upgrade ();
		}
	}


	public string getName() {
		return this.name;
	}

	public int getNumShields() {
		return this.numShields;
	}

	public List<Card> getHand() {
		return this.hand;
	}

	public Rank getRank() {
		return this.rank;
	}

	public void incrementShields(int numShields) {
		this.numShields = numShields;
		checkForRankUp ();
	}

	public void decrementShields(int numShields) {
		this.numShields = numShields;
	}

	public string toString() {
		string toString = this.name + " (" + (isAI ? "AI" : "human") + "): ";
		foreach (Card card in hand) {
			toString += card.toString() + ", ";
		}
		toString = toString.Substring (0, toString.Length - 2);
		return toString;
	}
}


