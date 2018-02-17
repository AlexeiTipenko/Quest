using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class BoardManagerMediator
{
	static BoardManagerMediator instance;
	GameObject boardManager;
	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;
	Story cardInPlay;
	int playerTurn;

	public GameObject cardPrefab;
	public GameObject board;
	public List<CardUI> cards = new List<CardUI>();


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
	}

	public List<Player> getPlayers() {
		return players;
	}

	public Player getCurrentPlayer() {
		return players [playerTurn];
	}

	public Player getNextPlayer(Player previousPlayer) {
		int index = players.IndexOf (previousPlayer);
		if (index != -1) {
			return players [(index + 1) % players.Count];
		}
		return null;
	}

	public Story getCardInPlay() {
		return cardInPlay;
	}

	public void dealCardsToPlayer(Player player, int numCardsToDeal) {
        Debug.Log("Dealing " + numCardsToDeal + " cards");
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

    public void setCardInPlay(Card card) {
        cardInPlay = (Story) card;
    }

	public void promptSponsorQuest(Player player) {
		//TODO: prompt sponsor quest
	}

	public void setupQuest(Player player) {
		//TODO: prompt setup quest
	}

	public void promptAcceptQuest(Player player) {
		//TODO: prompt accept quest
	}

	public void promptFoe(Player player) {
		//TODO: prompt foe
	}

	public void promptTest(Player player, int currentBid) {
		//TODO: prompt test
	}

	public void startGame() {
		playerTurn = 0;
		playTurn ();
	}

	private void playTurn() {
		if (!gameOver ()) {
			cardInPlay = (Story)storyDeck.drawCard ();
            BoardManager.DisplayCards(players[playerTurn]);
			cardInPlay.startBehaviour ();
		} else {
			//TODO: Game over!
		}
	}

	public void nextTurn() {
		storyDiscard.addCard (cardInPlay);
        BoardManager.DestroyCards(players[playerTurn]);
		cardInPlay = null;
		playerTurn = (playerTurn + 1) % players.Count;
		playTurn ();
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
            case "nextPlayer":
                Debug.Log("Current player is: " + players[playerTurn].getName());
                nextTurn();
                Debug.Log("New player is: " + players[playerTurn].getName());
                break;

			//TODO: Fix this! Event cards should not be able to be dealt to the player's hand
    		//case "prosperity":
    		//	Debug.Log ("Drawing Prosperity throughout the kingdom into current players hand");
    		//	ProsperityThroughoutTheRealm prospCard = new ProsperityThroughoutTheRealm ();
    		//	players [playerTurn].getHand ().Add (prospCard);
    		//	foreach (Card card in players[playerTurn].getHand()) {
    		//		Debug.Log (card.getCardName ());
    		//	}
    		//	break;
    		//case "chivalrous":
    			//Debug.Log ("Drawing Chivalrous Deeds into current players hand");
    			//ChivalrousDeed chivCard = new ChivalrousDeed ();
    			//players [playerTurn].getHand ().Add (chivCard);
    			//Debug.Log ("Listing current players hand");
    			//foreach (Card card in players[playerTurn].getHand()) {
    			//	Debug.Log (card.getCardName ());
    			//}
    			//break;
		}
	}
}

