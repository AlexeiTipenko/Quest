using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class AdventureDeckTest {

	[Test]
	public void testInitAdventureDeck() {
		Deck deck = new AdventureDeck ();
		Assert.IsTrue (deck.getCards ().Count == 125);
	}

	[Test]
	public void testCorrectDistributionOfCards() {
		Deck deck = new AdventureDeck ();
		List<string> cardNames = new List<string>();

		foreach (Card card in deck.getCards()) {
			cardNames.Add (card.cardImageName);
		}

		List<string> unique_cardNames = new List<string>(new HashSet<string>(cardNames));

		Assert.IsTrue (unique_cardNames.Count == 32); //assert that there are 32 unique adventure cards

		Dictionary<string, int> cardCountMap = cardNames.GroupBy( i => i ).ToDictionary(t => t.Key, t=> t.Count()); //first value is the name, second is the count

		int totalCardCount = 0;
		foreach (string name in unique_cardNames) {
			Type genericType = Type.GetType(name, true);
			int frequency = (int) genericType.GetField ("frequency").GetValue (null);

			Assert.IsTrue (cardCountMap[name] == frequency);
			totalCardCount += cardCountMap[name];
		}

		Assert.IsTrue (totalCardCount == 125);
	}

}
