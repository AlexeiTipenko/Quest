using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PoxTest {

	//Event description: All players except the player drawing this card lose 1 shield.
	[Test]
	public void PoxTestSimplePasses() {
		Assert.IsTrue (Pox.frequency == 1);
		Event pox = new Pox ();
		Assert.AreEqual ("Pox", pox.getCardName ());

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

		pox.startBehaviour ();

		Debug.Log ("Joey's # Shields: " + joey.getNumShields());
		Assert.IsTrue (joey.getNumShields() == 1);
		Debug.Log ("Julie's # Shields: " + julie.getNumShields());
		Assert.IsTrue (julie.getNumShields() == 4);
		Debug.Log ("Jimmy's # Shields: " + jimmy.getNumShields());
		Assert.IsTrue (jimmy.getNumShields() == 2);
		Debug.Log ("Jesse's # Shields: " + jesse.getNumShields());
		Assert.IsTrue (jesse.getNumShields() == 3);
	}
}
