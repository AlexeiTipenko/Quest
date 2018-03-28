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
        CourtCalledToCamelot courtCalledToCamelot = new CourtCalledToCamelot ();
		Assert.AreEqual ("Court Called to Camelot", courtCalledToCamelot.getCardName ());
	}

	[Test]
	public void CourtCalledToCamelotTestBehaviour() {
        CourtCalledToCamelot courtCalledToCamelot = new CourtCalledToCamelot ();

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

		//All players put down their allies

		players = BoardManagerMediator.getInstance ().getPlayers ();

		PlayerPlayArea playArea;

		foreach (Player player in players) {
			playArea = player.getPlayArea ();

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
