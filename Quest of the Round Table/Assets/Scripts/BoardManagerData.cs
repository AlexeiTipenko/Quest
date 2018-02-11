using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardManagerData
{
	static BoardManager boardManager;
	static List<Player> players;
	static AdventureDeck adventureDeck;
	static StoryDeck storyDeck;
	static DiscardDeck adventureDiscard, storyDiscard;
	static int playerTurn;

	public static void initGame (List<Player> players) {
		Debug.Log ("Received playersList");
		BoardManagerData.players = players;
		foreach (Player player in players) {
			Debug.Log (player.toString ());
		}

		adventureDeck = new AdventureDeck ();
		storyDeck = new StoryDeck ();
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();

		foreach (Player player in players) {
			dealCardsToPlayer (player, 12);
			Debug.Log (player.getName ());
			foreach (Card card in player.getHand()) {
				Debug.Log (card.toString ());
			}
		}

		playerTurn = 0;
	}

	public static List<Player> getPlayers() {
		return players;
	}

	public static Player getCurrentPlayer() {
		return players [playerTurn];
	}

	public static void dealCardsToPlayer(Player player, int numCardsToDeal) {
		List<Card> cardsToDeal = new List<Card> ();
		for (int i = 0; i < numCardsToDeal; i++) {
			cardsToDeal.Add (adventureDeck.drawCard ());
			if (adventureDeck.getSize () <= 0) {
				adventureDeck = new AdventureDeck (adventureDiscard);
				adventureDiscard.empty ();
			}
		}
		player.dealCards (cardsToDeal);
	}

	public static void gameLoop() {
		while (!gameOver ()) {
			//Draw story card from the deck
			Card storyCard = storyDeck.drawCard ();

			//Add the story card visually to the play area (not sure how to do this, Alexei can you take a look?)
			//Code here

			//Act on the story card
			storyCard.startBehaviour ();

			//End of turn, move to next player
			playerTurn = (playerTurn + 1) % players.Count;
		}
	}

	private static bool gameOver() {
		foreach (Player player in players) {
			if (player.getRank ().getCardName () == "Knight of the Round Table") {
				return true;
			}
		}
		return false;
	}
}

