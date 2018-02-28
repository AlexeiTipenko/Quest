using System;
using System.Collections.Generic;

/*
	player1 gets 12 cards including saxons, boar and sword
	players 2, 3, and 4 get 12 cards (with some specific ones as seen below)
	first story card is Boar Hunt
*/

public class Scenario2 {

	public static Scenario2 instance;
	private List<Player> players; 
	private AdventureDeck adventureDeck;
	private StoryDeck storyDeck;

	public static Scenario2 getInstance() {
		if (instance == null) {
			instance = new Scenario2 ();
		}
		return instance;
	}

	public Scenario2 () {
		players = BoardManagerMediator.getInstance ().getPlayers ();
	}

	public void setupScenario(AdventureDeck adventureDeck, StoryDeck storyDeck) {
		this.adventureDeck = adventureDeck;
		this.storyDeck = storyDeck;

		foreach (Player player in players) {
			player.removeCards (12);
		}

		//reinitialize adventure deck
		adventureDeck.initAdventureDeck ();

		List<List<string>> cardsToDrawByPlayer = new List<List<string>> ();
		List<string> p1ReqCards = new List<string> (new string[] {"Saxons", "Boar", "Sword", "Dagger"});
		List<string> p2ReqCards = new List<string> (new string[] {"Excalibur", "Sword"});
		List<string> p3ReqCards = new List<string> (new string[] {"Horse", "Excalibur", "Amour"});
		List<string> p4ReqCards = new List<string> (new string[] {"BattleAx", "Lance", "Thieves"});
		cardsToDrawByPlayer.Add (p1ReqCards);
		cardsToDrawByPlayer.Add (p2ReqCards);
		cardsToDrawByPlayer.Add (p3ReqCards);
		cardsToDrawByPlayer.Add (p4ReqCards);

		//hand out essential cards to player
		for (int playerNum = 0; playerNum < players.Count; playerNum++) {
			foreach (string cardStr in cardsToDrawByPlayer[playerNum]) {
				players[playerNum] = adventureDeck.drawCardByName (cardStr, players [playerNum]);
			}
		}

		BoardManagerMediator.getInstance ().setPlayers (players);
		BoardManagerMediator.getInstance ().setAdventureDeck (adventureDeck);

		//deal the rest of the cards so each player has 12 cards
		foreach (Player player in players) {
			BoardManagerMediator.getInstance().dealCardsToPlayer(player, (12 - player.getHand().Count));
		}

		//reinitialize story deck
		storyDeck.initStoryDeck ();

		Card boarHunt = storyDeck.getCardByName ("BoarHunt");
		if (boarHunt == null) {
			return;
		}

		boarHunt.setOwner (players [0]);
		BoardManagerMediator.getInstance ().setCardInPlay (boarHunt);
		storyDeck.moveCardToIndex ("DefendTheQueensHonor", 0);
		storyDeck.moveCardToIndex ("ChivalrousDeed", 1);

		BoardManagerMediator.getInstance ().setStoryDeck (storyDeck);

		BoardManager.DrawCards (players[0]);
		//		BoardManager.DestroyPlayerInfo();
		//		BoardManager.DisplayPlayers(players);
		BoardManagerMediator.getInstance ().getCardInPlay().startBehaviour();
	}
}
