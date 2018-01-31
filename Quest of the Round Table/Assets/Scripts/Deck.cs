using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck {

	protected List<Card> cards;
	protected int size;

	public Deck() {
		cards = new List<Card> ();
	}

	public Deck(Deck oldDeck) {
		cards = oldDeck.getCards ();
		shuffle ();
	}

	protected void shuffle() {

	}

	public List<Card> getCards() {
		return cards;
	}

	protected void instantiateCards(List<string> newCards) {
		for (int i = 0; i < newCards.Count; i++) {
			Type genericType = Type.GetType(newCards[i], true);
			int frequency = (int) genericType.GetField ("frequency").GetValue(null);
			for (int j = 0; j < frequency; j++) {
				cards.Add((Card) Activator.CreateInstance(genericType));
			}
		}
	}

	public string toString() {
		string toString = "Cards in Deck: ";
		foreach (Card card in cards) {
			toString += card.toString ();
		}
		toString = toString.Substring (0, toString.Length - 2);
		return toString;
	}

}
