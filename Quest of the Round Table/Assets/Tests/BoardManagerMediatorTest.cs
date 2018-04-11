using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class BoardManagerDataTest {

	[Test]
	public void testInitGame() {
		List<Player> playerList = new List<Player> ();
		playerList.Add(new HumanPlayer("Joey"));
		playerList.Add(new HumanPlayer("Julie"));
		playerList.Add(new HumanPlayer("Jimmy"));
		playerList.Add(new HumanPlayer("Jesse"));
		BoardManagerMediator.getInstance().initGame (playerList);
		foreach (Player player in playerList) {
			Debug.Log (player.toString ());
			Assert.AreEqual (player.GetHand ().Count, 12);
		}
	}
}
