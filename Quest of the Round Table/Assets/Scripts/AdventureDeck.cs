using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureDeck : Deck {

	public AdventureDeck () {
		initAdventureDeck ();
	}

	public AdventureDeck (Deck deck) : base (deck) {
			
	}

	public void initAdventureDeck() {
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
		newCards.Add ("SirGalahad");
		newCards.Add ("SirLancelot");
		newCards.Add ("KingArthur");
		newCards.Add ("SirTristan");
		newCards.Add ("KingPellinore");
		newCards.Add ("SirGawain");
		newCards.Add ("SirPercival");
		newCards.Add ("QueenGuinevere");
		newCards.Add ("QueenIseult");
		newCards.Add ("Merlin");

		//amours
		newCards.Add ("Amour");

		//weapons
		newCards.Add ("Excalibur");
		newCards.Add ("Lance");
		newCards.Add ("BattleAx");
		newCards.Add ("Sword");
		newCards.Add ("Horse");
		newCards.Add ("Dagger");

		instantiateCards (newCards);

		shuffle ();
	}

	public Player drawCardByName(string cardName, Player player) {
		int cardIndex = getCardIndexByName (cardName);
		if (cardIndex == -1) {
			Debug.LogError ("In drawCardByName: Card by the name " + cardName + " does not exist in the Adventure Deck.");
			Logger.getInstance().error ("In drawCardByName: Card by the name " + cardName + " does not exist in the Adventure Deck.");
			return player;
		}
		Card card = cards [cardIndex];
		cards.RemoveAt (cardIndex);
		player.dealCards (new List<Card>{card});
		card.setOwner (player);
		return player;
	}

	public override string toString() {
		string toString = "Adventure Deck: ";
		foreach (Card card in cards) {
			toString += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			toString = toString.Substring (0, toString.Length - 2);
		}
		return toString;
	}
}
