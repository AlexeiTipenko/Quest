using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BoardManager : MonoBehaviour {

	static List<Player> players;
	static AdventureDeck adventureDeck;
	static StoryDeck storyDeck;
	static DiscardDeck adventureDiscard, storyDiscard;
	int playerTurn;

	// Use this for initialization
	static void Start () {
		print ("Board manager started");
	}
	
	// Update is called once per frame
	static void Update () {
		
	}

	public static void initGame (List<Player> players) {
		print ("Received playersList");
		this.players = players;
		foreach (Player player in players) {
			print (player.toString ());
		}

		adventureDeck = new AdventureDeck ();
		storyDeck = new StoryDeck ();
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();

		foreach (Player player in players) {
			dealCardsToPlayer (player, 12);
			print (player.getName ());
			foreach (Card card in player.getHand()) {
				print (card.toString ());
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
