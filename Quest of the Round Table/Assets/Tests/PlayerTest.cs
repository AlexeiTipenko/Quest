using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PlayerTest {

	[Test]
	public void testDealCards() {
		Player player = new HumanPlayer ("Joey");

		List<Adventure> cards = new List<Adventure> ();
		cards.Add (new TestOfMorganLeFey ());
		cards.Add (new Merlin ());
		cards.Add (new BattleAx ());

		player.DealCards (cards, null);

		Assert.IsTrue (player.GetHand().Count == cards.Count);
	}

}
