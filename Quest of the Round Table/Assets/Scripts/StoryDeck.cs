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
}
