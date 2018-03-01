using System;
using System.Collections.Generic;

/*
	player1 gets 12 cards including saxons, boar and sword
	players 2, 3, and 4 get 12 cards (with some specific ones as seen below)
	first story card is Boar Hunt
*/

public class Scenario1 {

	public static Scenario1 instance;
	private List<Player> players; 
	private AdventureDeck adventureDeck;
	private StoryDeck storyDeck;

	public static Scenario1 getInstance() {
		if (instance == null) {
			instance = new Scenario1 ();
		}
		return instance;
	}

	public Scenario1 () {
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
		List<string> p2ReqCards = new List<string> (new string[] {"Horse", "Dagger"});
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
		for (int i = 0; i < players.Count; i++) {
			BoardManagerMediator.getInstance().dealCardsToPlayer(players[i], (12 - players[i].getHand().Count));
			Logger.getInstance ().debug (players[i].getName() + " has the following cards: " );
			//Logger.getInstance ().debug (players[i].getName() + " has cards " + BoardManagerMediator.getInstance ().getPlayers () [i].getHand ().ToString());
			foreach (Card card in BoardManagerMediator.getInstance ().getPlayers () [i].getHand ()) {
				Logger.getInstance ().trace (card.getCardName ());
			}
//			}
		}

		//reinitialize story deck
		storyDeck.initStoryDeck ();

		Card boarHunt = storyDeck.getCardByName ("BoarHunt");
		if (boarHunt == null) {
			//Debug.Log("Boar Hunt is null")
			return;
		}
	
		BoardManager.DestroyStages ();

		boarHunt.setOwner (players [0]);
		BoardManagerMediator.getInstance ().setCardInPlay (boarHunt);
		storyDeck.moveCardToIndex ("ProsperityThroughoutTheRealm", 0);
		storyDeck.moveCardToIndex ("ChivalrousDeed", 1);

		if (!BoardManager.QuestPanelsExist()) {
			BoardManager.SetupQuestPanels(((Quest) BoardManagerMediator.getInstance ().getCardInPlay()).getNumStages());
		}

		BoardManagerMediator.getInstance ().setStoryDeck (storyDeck);
		Logger.getInstance ().debug ("Moved ProsperityThroughoutTheRealm and ChivalrousDeed to the top of the Story Deck");

		BoardManager.DrawCards (players[0]);

//		BoardManager.DestroyPlayerInfo();
//		BoardManager.DisplayPlayers(players);
		BoardManagerMediator.getInstance ().getCardInPlay().startBehaviour();
	}
}
