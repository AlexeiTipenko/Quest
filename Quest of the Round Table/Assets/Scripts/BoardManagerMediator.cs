using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerMediator
{
	static BoardManagerMediator instance;
	BoardManager boardManager;
	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;
	int playerTurn;

	public static BoardManagerMediator getInstance() {
		if (instance == null) {
			instance = new BoardManagerMediator ();
		}
		return instance;
	}

	public void initGame (List<Player> players) {
		Debug.Log ("Received playersList");
		this.players = players;
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

	public List<Player> getPlayers() {
		return players;
	}

	public Player getCurrentPlayer() {
		return players [playerTurn];
	}

	public void dealCardsToPlayer(Player player, int numCardsToDeal) {
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

	public void gameLoop() {
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

	private bool gameOver() {
		foreach (Player player in players) {
			if (player.getRank ().getCardName () == "Knight of the Round Table") {
				return true;
			}
		}
		return false;
	}

	public void cheat(string cheatCode) {
		switch (cheatCode) {
		case "rankUp":
			Debug.Log ("Current player is: " + players [playerTurn].getName ());
			players [playerTurn].upgradeRank ();
			Debug.Log ("After upgrading rank: " + players [playerTurn].getRank ());
			break;
		case "shieldsUp":
			Debug.Log ("Player now has : " + players [playerTurn].getNumShields () + " shields");
			players [playerTurn].incrementShields (3);
			Debug.Log ("Player now has : " + players [playerTurn].getNumShields () + " after incremented shields");
			break;
		case "prosperity":
			Debug.Log ("Drawing Prosperity throughout the kingdom into current players hand");
			ProsperityThroughoutTheRealm prospCard = new ProsperityThroughoutTheRealm ();
			players [playerTurn].getHand ().Add (prospCard);
			foreach (Card card in players[playerTurn].getHand()) {
				Debug.Log (card.getCardName ());
			}
			break;
		case "chivalrous":
			Debug.Log ("Drawing Chivalrous Deeds into current players hand");
			ChivalrousDeed chivCard = new ChivalrousDeed ();
			players [playerTurn].getHand ().Add (chivCard);
			Debug.Log ("Listing current players hand");
			foreach (Card card in players[playerTurn].getHand()) {
				Debug.Log (card.getCardName ());
			}
			break;
		}
	}
}

