using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AdventureDeckTest {

	[Test]
	public void testInitAdventureDeck() {
		Deck deck = new AdventureDeck ();
		Debug.Log ("count is" + deck.getCards ().Count);
		Assert.IsTrue (deck.getCards ().Count == 67);
	}
}
