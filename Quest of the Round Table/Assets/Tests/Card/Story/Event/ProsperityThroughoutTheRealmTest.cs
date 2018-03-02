using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class ProsperityThroughoutTheRealmTest {

	//Event description: All players may immediately draw 2 Adventure Cards. 
	[Test]
	public void ProsperityThroughoutTheRealmTestSimplePasses() {
		Assert.IsTrue (ProsperityThroughoutTheRealm.frequency == 1);
		Event prosperityThroughoutTheRealm = new ProsperityThroughoutTheRealm ();
		Assert.AreEqual ("Prosperity Throughout the Realm", prosperityThroughoutTheRealm.getCardName ());
	}

	[Test]
	public void ProsperityThroughoutTheRealmTestBehaviour() {
		Event prosperityThroughoutTheRealm = new ProsperityThroughoutTheRealm ();

		List<Player> players = new List<Player>();

		Player joey = new HumanPlayer ("Joey");
		Player julie = new HumanPlayer("Julie");
		Player jimmy = new HumanPlayer ("Jimmy");
		Player jesse = new HumanPlayer ("Jesse");

		players.Add (joey);
		players.Add (julie);
		players.Add (jimmy);
		players.Add (jesse);

		BoardManagerMediator.getInstance().initGame (players);

        joey.RemoveRandomCards (7);
        julie.RemoveRandomCards (2);
        jimmy.RemoveRandomCards (11);
        jesse.RemoveRandomCards (8);

		Debug.Log ("Players are: " + BoardManagerMediator.getInstance().getPlayers ());

		prosperityThroughoutTheRealm.startBehaviour ();

		Debug.Log ("Joey's # Cards: " + joey.getHand().Count);
		Assert.IsTrue (joey.getHand().Count == 7);
		Debug.Log ("Julie's # Cards: " + julie.getHand().Count);
		Assert.IsTrue (julie.getHand().Count == 12);
		Debug.Log ("Jimmy's # Cards: " + jimmy.getHand().Count);
		Assert.IsTrue (jimmy.getHand().Count == 3);
		Debug.Log ("Jesse's # Cards: " + jesse.getHand().Count);
		Assert.IsTrue (jesse.getHand().Count == 6);
	}
}
