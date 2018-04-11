using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DiscardDeck : Deck {

	public void addCard (Card card) {
        Debug.Log("Discarded card: " + card.GetCardName());
		cards.Add (card);
	}

	public void empty () {
		cards = new List<Card> ();
	}

	public override string toString() {
		string output = "Discard Deck: ";
		foreach (Card card in cards) {
			output += (card.ToString () + ", ");
		}
		if (cards.Count > 0) {
			output = output.Substring (0, output.Length - 2);
		}
		return output;
	}
}
