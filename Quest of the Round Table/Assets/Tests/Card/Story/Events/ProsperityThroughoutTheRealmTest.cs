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
        ProsperityThroughoutTheRealm prosperityThroughoutTheRealm = new ProsperityThroughoutTheRealm ();
		Assert.AreEqual ("Prosperity Throughout the Realm", prosperityThroughoutTheRealm.GetCardName ());
	}

	[Test]
	public void ProsperityThroughoutTheRealmTestBehaviour() {
        ProsperityThroughoutTheRealm prosperityThroughoutTheRealm = new ProsperityThroughoutTheRealm ();

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

		Debug.Log ("Joey's # Cards: " + joey.GetHand().Count);
		Assert.IsTrue (joey.GetHand().Count == 7);
		Debug.Log ("Julie's # Cards: " + julie.GetHand().Count);
		Assert.IsTrue (julie.GetHand().Count == 12);
		Debug.Log ("Jimmy's # Cards: " + jimmy.GetHand().Count);
		Assert.IsTrue (jimmy.GetHand().Count == 3);
		Debug.Log ("Jesse's # Cards: " + jesse.GetHand().Count);
		Assert.IsTrue (jesse.GetHand().Count == 6);
	}
}
