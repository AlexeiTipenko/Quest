using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class StoryDeckTest {

	[Test]
	public void testInitStoryDeck() {
		Deck deck = new StoryDeck ();
		Assert.IsTrue (deck.getCards ().Count == 28);
	}

	[Test]
	public void testCorrectDistributionOfCards() {
		Deck deck = new StoryDeck ();
		List<string> cardNames = new List<string>();

		foreach (Card card in deck.getCards()) {
			cardNames.Add (card.cardImageName);
		}

		List<string> unique_cardNames = new List<string>(new HashSet<string>(cardNames));

		Debug.Log ("unique_cardnames count is " + unique_cardNames.Count);
		Assert.IsTrue (unique_cardNames.Count == 22); //assert that there are 22 unique story cards

		Dictionary<string, int> cardCountMap = cardNames.GroupBy( i => i ).ToDictionary(t => t.Key, t=> t.Count()); //first value is the name, second is the count

		int totalCardCount = 0;
		foreach (string name in unique_cardNames) {
			Type genericType = Type.GetType(name, true);
			int frequency = (int) genericType.GetField ("frequency").GetValue (null);

			Assert.IsTrue (cardCountMap[name] == frequency);
			totalCardCount += cardCountMap[name];
		}

		Assert.IsTrue (totalCardCount == 28);
	}
}
