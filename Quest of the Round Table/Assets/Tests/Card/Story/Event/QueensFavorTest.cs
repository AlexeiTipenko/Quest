using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class QueensFavorTest {

	//Event description: The lowest ranked player(s) immediately receives 2 Adventure Cards.
	[Test]
	public void QueensFavorTestSimplePasses() {
		Assert.IsTrue (QueensFavor.frequency == 2);
		Event queensFavor = new QueensFavor ();
		Assert.AreEqual ("Queen's Favor", queensFavor.getCardName ());
	}

	[Test]
	public void QueensFavorTestBehaviour() {
		Event queensFavor = new QueensFavor ();

		List<Player> players = new List<Player>();

		Player joey = new HumanPlayer ("Joey");
		Player julie = new HumanPlayer("Julie");
		Player jimmy = new HumanPlayer ("Jimmy");
		Player jesse = new HumanPlayer ("Jesse");

		players.Add (joey);
		players.Add (julie);
		players.Add (jimmy);
		players.Add (jesse);

		joey.upgradeRank ();

		//julie at first rank

		//jimmy at first rank

		jesse.upgradeRank ().upgradeRank ();

		BoardManagerMediator.getInstance().initGame (players);

		joey.removeCards (7);
		julie.removeCards (2);
		jimmy.removeCards (11);
		jesse.removeCards (8);

		Debug.Log ("Players are: " + BoardManagerMediator.getInstance().getPlayers ());

		queensFavor.startBehaviour ();

		Debug.Log ("Joey's # Cards: " + joey.getHand().Count);
		Assert.IsTrue (joey.getHand().Count == 5);
		Debug.Log ("Julie's # Cards: " + julie.getHand().Count);
		Assert.IsTrue (julie.getHand().Count == 12);
		Debug.Log ("Jimmy's # Cards: " + jimmy.getHand().Count);
		Assert.IsTrue (jimmy.getHand().Count == 3);
		Debug.Log ("Jesse's # Cards: " + jesse.getHand().Count);
		Assert.IsTrue (jesse.getHand().Count == 4);
	}
}
