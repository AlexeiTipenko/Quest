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


	void Start () {
		print ("Board manager started");
	}

	void Update () {
		if (Input.GetKeyUp ("r")) {
			Debug.Log ("Current player is: " + players [playerTurn].getName ());
			players [playerTurn].upgradeRank ();
			Debug.Log ("After upgrading rank: " + players [playerTurn].getRank ());
		} else if (Input.GetKeyUp ("s")) {
			Debug.Log ("Player now has : " + players [playerTurn].getNumShields () + " shields");
			players [playerTurn].incrementShields (3);
			Debug.Log ("Player now has : " + players [playerTurn].getNumShields () + " after incremented shields");
		} else if (Input.GetKeyUp ("p")) {
			Debug.Log ("Drawing Prosperity throughout the kingdom into current players hand");
			ProsperityThroughoutTheRealm prospCard = new ProsperityThroughoutTheRealm ();
			players [playerTurn].getHand ().Add (prospCard);
			foreach (Card card in players[playerTurn].getHand()) {
				Debug.Log (card.getCardName ());
			}
		} else if (Input.GetKeyUp ("c")) {
			Debug.Log ("Drawing Chivalrous Deeds into current players hand");
			ChivalrousDeed chivCard = new ChivalrousDeed ();
			players [playerTurn].getHand ().Add (chivCard);
			Debug.Log ("Listing current players hand");
			foreach (Card card in players[playerTurn].getHand()) {
				Debug.Log (card.getCardName ());
			}
		}
	}

	public static void initGame (List<Player> players) {
		print ("Received playersList");
		this.players = players;
		foreach (Player player in this.players) {
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
			storyCard.startBehaviour();

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
