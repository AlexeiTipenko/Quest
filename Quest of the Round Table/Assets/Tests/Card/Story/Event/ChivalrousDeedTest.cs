using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class ChivalrousDeedTest {

	//Event description: Player with both lowest rank and least amount of shields, receives 3 shields.
	[Test]
	public void ChivalrousDeedTestSimplePasses() {
		Assert.IsTrue (ChivalrousDeed.frequency == 1);
		ChivalrousDeed chivalrousDeed = new ChivalrousDeed ();
		Assert.AreEqual ("Chivalrous Deed", chivalrousDeed.getCardName ());
	}

	[Test]
	public void ChivalrousDeedTestBehaviour() {
		ChivalrousDeed chivalrousDeed = new ChivalrousDeed ();

		List<Player> players = new List<Player>();

		Player joey = new HumanPlayer ("Joey");
		Player julie = new HumanPlayer("Julie");
		Player jimmy = new HumanPlayer ("Jimmy");
		Player jesse = new HumanPlayer ("Jesse");

		joey.upgradeRank ().upgradeRank ();
		joey.incrementShields (4);

		julie.upgradeRank ();
		julie.incrementShields (5);

		jimmy.incrementShields (1);

		jesse.upgradeRank ().upgradeRank ();
		jesse.incrementShields (4);

		players.Add (joey);
		players.Add (julie);
		players.Add (jimmy);
		players.Add (jesse);

		BoardManagerMediator.getInstance().initGame (players);

		Debug.Log ("Players are: " + BoardManagerMediator.getInstance().getPlayers ());

		chivalrousDeed.startBehaviour ();

		players = BoardManagerMediator.getInstance ().getPlayers ();

		joey = players [0];
		julie = players [1];
		jimmy = players [2];
		jesse = players [3];

		Debug.Log ("Joey's Battle Points: " + joey.getRank ().getBattlePoints ());
		Debug.Log ("Joey's # Shields: " + joey.getNumShields());
		Assert.IsTrue (joey.getRank ().getBattlePoints () == 20);
		Assert.IsTrue (joey.getNumShields() == 4);
		Debug.Log ("Julie's Battle Points: " + julie.getRank ().getBattlePoints ());
		Debug.Log ("Julie's # Shields: " + julie.getNumShields());
		Assert.IsTrue (julie.getRank ().getBattlePoints () == 10);
		Assert.IsTrue (julie.getNumShields() == 5);
		Debug.Log ("Jimmy's Battle Points: " + jimmy.getRank ().getBattlePoints ());
		Debug.Log ("Jimmy's # Shields: " + jimmy.getNumShields());
		Assert.IsTrue (jimmy.getRank ().getBattlePoints () == 5);
		Assert.IsTrue (jimmy.getNumShields() == 4);
		Debug.Log ("Jesse's Battle Points: " + jesse.getRank ().getBattlePoints ());
		Debug.Log ("Jesse's # Shields: " + jesse.getNumShields());
		Assert.IsTrue (jesse.getRank ().getBattlePoints () == 20);
		Assert.IsTrue (jesse.getNumShields() == 4);
	}
}
