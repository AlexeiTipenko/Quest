using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PlagueTest {

	//Event description: Drawer loses 2 shields if possible.
	[Test]
	public void PlagueTestSimplePasses() {
		Assert.IsTrue (Plague.frequency == 1);
		Event plague = new Plague ();
		Assert.AreEqual ("Plague", plague.getCardName ());

		List<Player> players = new List<Player>();

		Player joey = new Player ("Joey", false);
		Player julie = new Player("Julie", false);
		Player jimmy = new Player ("Jimmy", false);
		Player jesse = new Player ("Jesse", false);

		players.Add (joey);
		players.Add (julie);
		players.Add (jimmy);
		players.Add (jesse);

		joey.upgradeRank ().upgradeRank ();
		joey.incrementShields (1);

		julie.upgradeRank ();
		julie.incrementShields (5);

		jimmy.incrementShields (3);

		jesse.upgradeRank ().upgradeRank ();
		jesse.incrementShields (4);

		BoardManagerMediator.getInstance().initGame (players);

		Debug.Log ("Players are: " + BoardManagerMediator.getInstance().getPlayers ());

		plague.startBehaviour ();

		Debug.Log ("Joey's # Shields: " + joey.getNumShields());
		Assert.IsTrue (joey.getNumShields() == 0);
		Debug.Log ("Julie's # Shields: " + julie.getNumShields());
		Assert.IsTrue (julie.getNumShields() == 5);
		Debug.Log ("Jimmy's # Shields: " + jimmy.getNumShields());
		Assert.IsTrue (jimmy.getNumShields() == 3);
		Debug.Log ("Jesse's # Shields: " + jesse.getNumShields());
		Assert.IsTrue (jesse.getNumShields() == 4);

//
//		Debug.Log ("Joey's # Shields: " + joey.getNumShields());
//		Assert.IsTrue (joey.getNumShields() == 0);
//		Debug.Log ("Julie's # Shields: " + julie.getNumShields());
//		Assert.IsTrue (julie.getNumShields() == 3);
//		Debug.Log ("Jimmy's # Shields: " + jimmy.getNumShields());
//		Assert.IsTrue (jimmy.getNumShields() == 3);
//		Debug.Log ("Jesse's # Shields: " + jesse.getNumShields());
//		Assert.IsTrue (jesse.getNumShields() == 4);
	}
}
