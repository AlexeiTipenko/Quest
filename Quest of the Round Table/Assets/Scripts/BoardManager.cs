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

	public GameObject cardPrefab;
	public GameObject board;
	public List<CardUI> cards = new List<CardUI>();

	void Start () {
		print ("Board manager started");

		//Adding prefab to hand area
		/*
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
			*/
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
			print (".......");
			dealCardsToPlayer (player, adventureDeck, 12);
			print (player.getName ());
			foreach (Card card in player.getHand()) {
				print (card.toString ());
			}
		}

		playerTurn = 0;
	}
		

	private void dealCardsToPlayer(Player player, AdventureDeck sourceDeck, int numCardsToDeal) {

		print ("Entered private method...");

		var handArea = GameObject.Find("HandArea").transform;
		List<Card> cardsToDeal = new List<Card> ();

		for (int i = 0; i < numCardsToDeal; i++) {

			Card card = sourceDeck.drawCard ();
			cardsToDeal.Add(card);

			print ("Loading prefab...");
			GameObject cardPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Allies/KingArthur.prefab",
				typeof(GameObject)) as GameObject;
			
			var cardObj = GameObject.Instantiate(cardPrefab);
			cardObj.AddComponent<Image>();
			cardObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			cardObj.AddComponent<CardUI>();
			cardObj.GetComponent<CardUI> ().CreateCard(card.getCardName());
			cardObj.transform.SetParent (handArea.transform, false);
			print ("Card should be in hand.");

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
