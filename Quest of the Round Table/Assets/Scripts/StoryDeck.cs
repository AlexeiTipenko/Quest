using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDeck : Deck {

	public StoryDeck () {
		initStoryDeck ();
	}

	private void initStoryDeck() {
		List<string> newCards = new List<string> ();

		//tournaments
		newCards.Add ("AtOrkney");
		newCards.Add ("AtCamelot");
		newCards.Add ("AtTintagel");
		newCards.Add ("AtYork");

		//events
		//insert events here

		//quests
		//insert quests here

		instantiateCards (newCards);

		shuffle ();
	}

	public override string toString() {
		string toString = "Story Deck: ";
		foreach (Card card in cards) {
			toString += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			toString = toString.Substring (0, toString.Length - 2);
		}
		return toString;
	}
}
