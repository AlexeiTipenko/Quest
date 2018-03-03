using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DeckTest {

	[Test]
	public void testShuffle() {
		Deck deck = new AdventureDeck ();
		Deck shuffledDeck = new AdventureDeck (deck);
		Assert.AreNotEqual (deck.getCards (), shuffledDeck.getCards ());
	}

}
