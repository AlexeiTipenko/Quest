using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class CourtCalledToCamelotTest {

	//Event description: All Allies in play must be discarded.
	[Test]
	public void CourtCalledToCamelotTestSimplePasses() {
		Assert.IsTrue (CourtCalledToCamelot.frequency == 2);
		Event courtCalledToCamelot = new CourtCalledToCamelot ();
		Assert.AreEqual ("Court Called to Camelot", courtCalledToCamelot.getCardName ());
	}

	[Test]
	public void CourtCalledToCamelotTestBehaviour() {
		Event courtCalledToCamelot = new CourtCalledToCamelot ();

		List<Player> players = new List<Player>();

		Player joey = new Player ("Joey", false);
		Player julie = new Player("Julie", false);
		Player jimmy = new Player ("Jimmy", false);
		Player jesse = new Player ("Jesse", false);

		players.Add (joey);
		players.Add (julie);
		players.Add (jimmy);
		players.Add (jesse);

		BoardManagerMediator.getInstance().initGame (players);

		//All players put down their allies

		players = BoardManagerMediator.getInstance ().getPlayers ();

		PlayerPlayArea playArea;
		List<Card> playerHand;

		foreach (Player player in players) {
			playArea = player.getPlayArea ();
			playerHand = player.getHand ();

			foreach (Card card in player.getHand()) {
				if (card.GetType ().IsSubclassOf (typeof(Ally))) {
					playArea.addCard (card);
				}
			}
			Debug.Log ("# card in the area for " + player.getName() +  " is " + player.getPlayArea ().getCards ().Count);
		}

		courtCalledToCamelot.startBehaviour ();

		foreach (Player player in players) {
			Assert.IsTrue (player.getPlayArea ().getCards ().Count == 0);
			Debug.Log (player.getName() + " # cards in play area are: " + player.getPlayArea ().getCards ().Count);
		}
	}
}
