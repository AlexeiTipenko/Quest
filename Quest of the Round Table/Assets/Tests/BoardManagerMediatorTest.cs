using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class BoardManagerDataTest {

	[Test]
	public void testInitGame() {
		List<Player> playerList = new List<Player> ();
		playerList.Add(new Player("Joey", false));
		playerList.Add(new Player("Julie", false));
		playerList.Add(new Player("Jimmy", false));
		playerList.Add(new Player("Jesse", false));
		BoardManagerMediator.getInstance().initGame (playerList);
		foreach (Player player in playerList) {
			Debug.Log (player.toString ());
			Assert.AreEqual (player.getHand ().Count, 12);
		}
	}
}
