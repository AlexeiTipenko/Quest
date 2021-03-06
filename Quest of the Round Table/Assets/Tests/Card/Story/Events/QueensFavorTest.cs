﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class QueensFavorTest {

	//Event description: The lowest ranked player(s) immediately receives 2 Adventure Cards.
	[Test]
	public void QueensFavorTestSimplePasses() {
		Assert.IsTrue (QueensFavor.frequency == 2);
        QueensFavor queensFavor = new QueensFavor ();
		Assert.AreEqual ("Queen's Favor", queensFavor.GetCardName ());
	}

	[Test]
	public void QueensFavorTestBehaviour() {
        QueensFavor queensFavor = new QueensFavor ();

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

        joey.RemoveRandomCards (7);
        julie.RemoveRandomCards (2);
        jimmy.RemoveRandomCards (11);
        jesse.RemoveRandomCards (8);

		Debug.Log ("Players are: " + BoardManagerMediator.getInstance().getPlayers ());

		queensFavor.startBehaviour ();

		Debug.Log ("Joey's # Cards: " + joey.GetHand().Count);
		Assert.IsTrue (joey.GetHand().Count == 5);
		Debug.Log ("Julie's # Cards: " + julie.GetHand().Count);
		Assert.IsTrue (julie.GetHand().Count == 12);
		Debug.Log ("Jimmy's # Cards: " + jimmy.GetHand().Count);
		Assert.IsTrue (jimmy.GetHand().Count == 3);
		Debug.Log ("Jesse's # Cards: " + jesse.GetHand().Count);
		Assert.IsTrue (jesse.GetHand().Count == 4);
	}
}
