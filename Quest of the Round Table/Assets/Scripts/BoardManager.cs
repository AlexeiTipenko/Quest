using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;
	int playerTurn;

	// Use this for initialization
	void Start () {
		print ("Board manager started");

		//Adding prefab to hand area
		print ("Loading prefab...");
		GameObject cardPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Allies/Merlin.prefab", typeof(GameObject)) as GameObject;

		var handArea = GameObject.Find("HandArea").transform;
		if (handArea != null) {
			var card = GameObject.Instantiate (cardPrefab);

			//Add Image Component to it(This will add RectTransform as-well)
			card.AddComponent<Image>();

			//Center Image to screen
			card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			card.transform.SetParent (handArea.transform, false);
			print ("Prefab should be in hand.");
		}
		else
			print ("Prefab has not been found.");


	}

	// Update is called once per frame
	void Update () {
		
	}

	public void initGame (List<Player> players) {
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
				//add cards to hand area
				print (card.toString ());
			}
		}

		playerTurn = 0;
	}

	public List<Player> getPlayers() {
		return this.players;
	}

	public Player getCurrentPlayer() {
		return players [playerTurn];
	}

	public int getCurrentPlayerTurn() {
		return playerTurn;
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
			Card storyCard = storyDeck.drawCard();
	

			// need to add cards to hand area here

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
}
