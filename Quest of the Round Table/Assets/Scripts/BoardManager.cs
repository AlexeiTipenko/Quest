using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour {

	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;
	int playerTurn;

	void Start () {
		print ("Board manager started");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			//Debug.Log ("Listening");
			Debug.Log("Current player is: " + players [playerTurn].getName ());
			players [playerTurn].upgradeRank ();
			Debug.Log("After upgrading rank: " + players [playerTurn].getRank ());
		}
		else if (Input.GetKeyDown(KeyCode.T)){
			//Debug.Log ("Listening");
		}
	}

	public void initGame (List<Player> players) {
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
			dealCardsToPlayer (player, adventureDeck, 12);
			print (player.getName ());
			foreach (Card card in player.getHand()) {
				print (card.toString ());
			}
		}

		playerTurn = 0;
	}

	private void dealCardsToPlayer(Player player, AdventureDeck sourceDeck, int numCardsToDeal) {
		List<Card> cardsToDeal = new List<Card> ();
		for (int i = 0; i < numCardsToDeal; i++) {
			cardsToDeal.Add (sourceDeck.drawCard ());
			if (sourceDeck.getSize () <= 0) {
				sourceDeck = new AdventureDeck (adventureDiscard);
				adventureDiscard.empty ();
			}
		}
		player.dealCards (cardsToDeal);
	}

	public void gameLoop() {
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

	private bool gameOver() {
		foreach (Player player in players) {
			if (player.getRank ().getCardName () == "Knight of the Round Table") {
				return true;
			}
		}
		return false;
	}

}
