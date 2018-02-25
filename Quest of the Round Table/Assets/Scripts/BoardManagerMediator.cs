using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class BoardManagerMediator
{
    
	static BoardManagerMediator instance;
	//GameObject boardManager;
    public static bool buttonClicked;
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
		//Debug.Log ("Received playersList");
		this.players = players;
        buttonClicked = false;

		adventureDeck = new AdventureDeck ();
		storyDeck = new StoryDeck ();
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();

		foreach (Player player in players) {
			dealCardsToPlayer (player, 12);
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

    public List<Card> GetSelectedCards(Player player) {
        
        List<string> cardNames = BoardManager.GetSelectedCardNames();
        List<Card> cardList = new List<Card>();

        foreach(string name in cardNames){
            foreach (Card card in player.getHand()) {
                if (card.getCardName() == name) {
                    cardList.Add(card);
                    break;
                }
            }
            //cardList.Add(player.getHand().Find(c => c.getCardName() == name));
        }
        return cardList;
    }

    public List<Card> GetDiscardedCards(Player player)
    {

        List<string> cardNames = BoardManager.GetSelectedDiscardNames();
        List<Card> cardList = new List<Card>();

        foreach (string name in cardNames)
        {
            foreach (Card card in player.getHand())
            {
                if (card.getCardName() == name)
                {
                    cardList.Add(card);
                    break;
                }
            }
            //cardList.Add(player.getHand().Find(c => c.getCardName() == name));
        }
        return cardList;
    }

    public void ReturnCardsToPlayer(){
        BoardManager.ReturnCardsToPlayer();
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

    public void dealOneCardToPlayer(Player player, Action func)
    {
        Debug.Log("Dealing " + 1 + " card");
        Card card = adventureDeck.drawCard();
        player.dealCard(card, func);
    }

    public void setCardInPlay(Card card) {
        cardInPlay = (Story) card;
    }

    public void startGame()
    {
        playerTurn = 0;
        playTurn();
    }

    private void playTurn()
    {
        if (!gameOver())
        {
            cardInPlay = (Story)storyDeck.drawCard();
            BoardManager.DrawCards(players[playerTurn]);
            cardInPlay.startBehaviour();
        }
        else
        {
            //TODO: Game over!
        }
    }

    public void nextTurn()
    {
        storyDiscard.addCard(cardInPlay);
        BoardManager.DestroyCards();
        BoardManager.ClearInteractions();
        cardInPlay = null;
        playerTurn = (playerTurn + 1) % players.Count;
        playTurn();
    }

    private bool gameOver()
    {
        foreach (Player player in players)
        {
            if (player.getRank().getCardName() == "Knight of the Round Table")
            {
                return true;
            }
        }
        return false;
    }

    public void cheat(string cheatCode)
    {
        switch (cheatCode)
        {
            case "rankUp":
                Debug.Log("Current player is: " + players[playerTurn].getName());
                players[playerTurn].upgradeRank();
                Debug.Log("After upgrading rank: " + players[playerTurn].getRank());
                break;
            case "shieldsUp":
                Debug.Log("Player now has : " + players[playerTurn].getNumShields() + " shields");
                players[playerTurn].incrementShields(3);
                Debug.Log("Player now has : " + players[playerTurn].getNumShields() + " after incremented shields");
                break;
            case "nextPlayer":
                Debug.Log("Current player is: " + players[playerTurn].getName());
                nextTurn();
                Debug.Log("New player is: " + players[playerTurn].getName());
                break;

                //TODO: Fix this! Event cards should not be able to be dealt to the player's hand
                //case "prosperity":
                //  Debug.Log ("Drawing Prosperity throughout the kingdom into current players hand");
                //  ProsperityThroughoutTheRealm prospCard = new ProsperityThroughoutTheRealm ();
                //  players [playerTurn].getHand ().Add (prospCard);
                //  foreach (Card card in players[playerTurn].getHand()) {
                //      Debug.Log (card.getCardName ());
                //  }
                //  break;
                //case "chivalrous":
                //Debug.Log ("Drawing Chivalrous Deeds into current players hand");
                //ChivalrousDeed chivCard = new ChivalrousDeed ();
                //players [playerTurn].getHand ().Add (chivCard);
                //Debug.Log ("Listing current players hand");
                //foreach (Card card in players[playerTurn].getHand()) {
                //  Debug.Log (card.getCardName ());
                //}
                //break;
        }
    }

    //------------------------------------------------------------------------//
    //--------------------------- Visual Functions ---------------------------//
    //------------------------------------------------------------------------//

    public void DrawRank(Player player) {
        BoardManager.DrawRank(player);
    }

	public void PromptSponsorQuest(Player player) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Would you like to sponsor this quest?");
        Action action1 = () => {
            ((Quest)cardInPlay).PromptSponsorQuestResponse(true);
        };
        Action action2 = () => {
            ((Quest)cardInPlay).PromptSponsorQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to sponsor quest.");
	}

	public void SetupQuest(Player player, Action action1) {
        BoardManager.SetInteractionText("Prepare your quest using a combination of foes (and weapons) and a test.");
        Debug.Log(((Quest)cardInPlay).numStages);

        //Generate panels
        BoardManager.SetupQuestPanels(((Quest)cardInPlay).numStages);

        BoardManager.SetInteractionButtons("Complete", "", action1, null);
        Debug.Log("Prompting " + player.getName() + " to setup quest.");
	}

	public void PromptAcceptQuest(Player player) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Would you like to participate in this quest?");
        Action action1 = () => {
            ((Quest)cardInPlay).PromptAcceptQuestResponse(true);
        };
        Action action2 = () => {
            ((Quest)cardInPlay).PromptAcceptQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to accept quest.");
	}

	public void PromptFoe(Player player) {
		//TODO: prompt foe
	}

	public void PromptTest(Player player, int currentBid) {
		//TODO: prompt test
	}


    public void PromptEnterTournament(Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Would you like to enter this tournament?");
        Action action1 = () => {
            ((Tournament)cardInPlay).PromptEnterTournamentResponse(true);
        };
        Action action2 = () => {
            ((Tournament)cardInPlay).PromptEnterTournamentResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to enter tournament.");
    }

    public void PromptCardSelection(Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Prepare for the tournament using a combination of weapon, ally and amour cards.");
        Action action = () => {
            ((Tournament)cardInPlay).CardsSelectionResponse();
        };
        BoardManager.SetInteractionButtons("Complete", "", action, null);
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");
    }

    public void PromptCardRemoveSelection(Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Please remove cards until you have at most 12.");

        Action action = () => {
            buttonClicked = true;
            BoardManager.DestroyDiscardArea();
            player.RemoveCardsResponse();
            player.PromptNextPlayer();
        };

        BoardManager.SetInteractionButtons("Complete", "", action, null);
        BoardManager.SetupDiscardPanel();
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");

        //BoardManager.WaitUntilButtonClick(buttonClicked);
        //coroutine = Coroutine(buttonClicked);
        //StartCoroutine(coroutine);
    }
}

