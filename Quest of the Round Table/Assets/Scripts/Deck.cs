using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Deck {

	private static System.Random rand;

	protected List<Card> cards;
	protected int size;

	public Deck() {
		if (rand == null) {
			rand = new System.Random ();
		}
		cards = new List<Card> ();
	}

	public Deck(Deck oldDeck) : this () {
		foreach (Card card in oldDeck.getCards()) {
			cards.Add(card);
		}
		shuffle ();
	}

	protected void shuffle() {
		for (int i = 0; i < cards.Count - 1; i++) {
			int j = rand.Next (i, cards.Count - 1);
			Card tempCard = cards [i];
			cards [i] = cards [j];
			cards [j] = tempCard;
		}
	}

	public List<Card> getCards() {
		return cards;
	}

	public int getSize() {
		return cards.Count;
	}

	public Card drawCard () {
		Card card = cards [0];
		cards.RemoveAt (0);
		card.setOwner (BoardManagerMediator.getInstance ().getCurrentPlayer ());
		return card;
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

	public virtual string toString() {
		string toString = "Deck: ";
		foreach (Card card in cards) {
			toString += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			toString = toString.Substring (0, toString.Length - 2);
		}
		return toString;
	}

}
