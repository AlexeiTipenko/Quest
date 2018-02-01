using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class BoardManagerTest {

	[Test]
	public void testInitGame() {
		GameObject gameObject = new GameObject ();
		gameObject.AddComponent<BoardManager> ();
		List<Player> playerList = new List<Player> ();
		playerList.Add(new Player("Joey", false));
		playerList.Add(new Player("Julie", false));
		playerList.Add(new Player("Jimmy", false));
		playerList.Add(new Player("Jesse", false));
		gameObject.GetComponent<BoardManager> ().initGame (playerList);
		foreach (Player player in playerList) {
			Debug.Log (player.toString ());
			Assert.AreEqual (player.getHand ().Count, 12);
		}
	}
}
