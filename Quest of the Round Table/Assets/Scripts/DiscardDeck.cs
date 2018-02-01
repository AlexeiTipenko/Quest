using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : Deck {

	public DiscardDeck () {

	}

	public void addCard (Card card) {
		cards.Add (card);
	}

	public void empty () {
		cards = new List<Card> ();
	}

	public override string toString() {
		string toString = "Discard Deck: ";
		foreach (Card card in cards) {
			toString += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			toString = toString.Substring (0, toString.Length - 2);
		}
		return toString;
	}
}
