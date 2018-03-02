using System.Collections.Generic;

public class StoryDeck : Deck {

	public StoryDeck () {
		initStoryDeck ();
	}

	public void initStoryDeck() {
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
		newCards.Add ("SearchForTheHolyGrail");
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

	public Card getCardByName(string cardName) {
		for (int i = 0; i < cards.Count; i++) {
			if (cards [i].cardImageName.Equals (cardName)) {
				return cards[i];
			}
		} 
		return null;
	}

	public override string toString() {
		string output = "Story Deck: ";
		foreach (Card card in cards) {
			output += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			output = output.Substring (0, output.Length - 2);
		}
		return output;
	}
}
