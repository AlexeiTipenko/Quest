using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PlayerTest {

	[Test]
	public void testDealCards() {
		Player player = new Player ("Joey", false);

		List<Card> cards = new List<Card> ();
		cards.Add (new AtCamelot ());
		cards.Add (new Merlin ());
		cards.Add (new BattleAxe ());

		player.dealCards (cards);

		Assert.IsTrue (player.getHand().Count == cards.Count);
	}

}
