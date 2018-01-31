using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureDeck : Deck {

	public AdventureDeck () {
		initAdventureDeck ();
	}

	public AdventureDeck (Deck deck) : base (deck) {
			
	}

	private void initAdventureDeck() {
		List<string> newCards = new List<string> ();

		//foes
		newCards.Add ("Dragon");
		newCards.Add ("Giant");
		newCards.Add ("Mordred");
		newCards.Add ("GreenKnight");
		newCards.Add ("BlackKnight");
		newCards.Add ("EvilKnight");
		newCards.Add ("SaxonKnight");
		newCards.Add ("RobberKnight");
		newCards.Add ("Saxons");
		newCards.Add ("Boar");
		newCards.Add ("Thieves");

		//tests
		newCards.Add ("TestOfValor");
		newCards.Add ("TestOfTemptation");
		newCards.Add ("TestOfMorganLeFey");
		newCards.Add ("TestOfTheQuestingBeast");

		//allies
		newCards.Add ("SirTristan");

		//amours
		newCards.Add ("Amour");

		//weapons
		//insert weapons here

		instantiateCards (newCards);

		shuffle ();
	}
}
