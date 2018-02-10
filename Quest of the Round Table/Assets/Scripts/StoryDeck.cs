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
		newCards.Add ("KingsRecognition");
		newCards.Add ("QueensFavor");
		newCards.Add ("CourtCalledToCamelot");
		newCards.Add ("Pox");
		newCards.Add ("Plague");
		newCards.Add ("ChivalrousDeed");
		newCards.Add ("ProsperityThroughoutTheRealm");
		newCards.Add ("KingsCallToArms");

		//quests
		newCards.Add("SearchForTheHolyGrail");
		newCards.Add ("TestOfTheGreenKnight");
		newCards.Add ("SearchForTheQuestingBeast");
		newCards.Add ("DefendTheQueensHonor");
		newCards.Add ("RescueTheFairMaiden");
		newCards.Add ("JourneyThroughTheEnchantedForest");
		newCards.Add ("VanquishKingArthursEnemies");
		newCards.Add ("SlayTheDragon");
		newCards.Add ("BoarHunt");
		newCards.Add ("RepelTheSaxonRaiders");

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
