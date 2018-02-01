using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class StoryDeckTest {

	[Test]
	public void testInitStoryDeck() {
		Deck deck = new StoryDeck ();
		Assert.IsTrue (deck.getCards ().Count == 4);
	}
}
