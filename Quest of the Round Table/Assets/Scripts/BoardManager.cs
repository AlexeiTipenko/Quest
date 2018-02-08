using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour {

	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;

	// Use this for initialization
	void Start () {
		print ("Board manager started");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void initGame (List<Player> players) {
		print ("Received playersList");
		foreach (Player player in players) {
			print (player.toString ());
		}
		this.players = players;

		adventureDeck = new AdventureDeck ();
		storyDeck = new StoryDeck ();
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();

		foreach (Player player in players) {
			dealCardsToPlayer (player, adventureDeck, 12);
		}
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
}
