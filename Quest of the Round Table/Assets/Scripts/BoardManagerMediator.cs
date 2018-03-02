using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerMediator
{
    
	static BoardManagerMediator instance;
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
		this.players = players;
        if (ButtonManager.scenario == "scenario1") {
            adventureDeck = (AdventureDeck)Scenario1.getInstance().AdventureDeck();
            storyDeck = (StoryDeck)Scenario1.getInstance().StoryDeck();
        } else if (ButtonManager.scenario == "scenario2") {
            adventureDeck = (AdventureDeck)Scenario2.getInstance().AdventureDeck();
            storyDeck = (StoryDeck)Scenario2.getInstance().StoryDeck();
        } else if (ButtonManager.scenario == "scenario3") {
            //TODO: scenario 3 here
        } else {
            adventureDeck = new AdventureDeck();
            storyDeck = new StoryDeck();
        }
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();
        Logger.getInstance().info("Card decks created");

		foreach (Player player in players) {
			dealCardsToPlayer (player, 12);
		}
	}

	public List<Player> getPlayers() {
		return players;
	}

	public void setAdventureDeck(AdventureDeck adventureDeck) {
		this.adventureDeck = adventureDeck;
	}

	public void setStoryDeck(StoryDeck storyDeck) {
		this.storyDeck = storyDeck;
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
        }
        return cardList;
    }

    public int GetCardsNumHandArea(Player player){
        return BoardManager.GetCardsNumHandArea(player);
    }

    public void ReturnCardsToPlayer(){
        BoardManager.ReturnCardsToPlayer();
    }

	public void dealCardsToPlayer(Player player, int numCardsToDeal) {
        Debug.Log("Dealing " + numCardsToDeal + " cards to player: " + player.getName());
		List<Card> cardsToDeal = new List<Card> ();
		for (int i = 0; i < numCardsToDeal; i++) {
			cardsToDeal.Add (adventureDeck.drawCard ());
			if (adventureDeck.getSize () <= 0) {
				adventureDeck = new AdventureDeck (adventureDiscard);
				adventureDiscard.empty ();
			}
		}
		player.dealCards (cardsToDeal);
        Logger.getInstance().info("Dealt " + numCardsToDeal + " cards to " + player.getName());
        if (player.GetType() == typeof(AIPlayer)) {
            Action action = player.getAction();
            if (action != null) {
                action.Invoke();
                player.giveAction(null);
            }
        }
	}

    public void setCardInPlay(Card card) {
        cardInPlay = (Story) card;
    }

	//used for scenarios, nowhere anywhere else
	public void setPlayers(List<Player> players) {
		this.players = players;
	}

    public void AddToDiscardDeck(Card card) {
        if (card.GetType().IsSubclassOf(typeof(Story))) {
            storyDiscard.addCard(card);
        } else {
            adventureDiscard.addCard(card);
        }
    }

    public void startGame()
    {
        Logger.getInstance().info("Game started...");
        playerTurn = 0;
        playTurn();
    }


    private void playTurn()
    {
        if (!gameOver())
        {
            Logger.getInstance().info(players[playerTurn].getName().ToUpper() + "'S TURN");
            Debug.Log(players[playerTurn].getName().ToUpper() + "'S TURN");
            if(storyDeck.getSize() <= 0) {
                storyDeck = new StoryDeck();
            }
            cardInPlay = (Story)storyDeck.drawCard();
            Debug.Log("Drew card: " + cardInPlay.getCardName());
            cardInPlay.startBehaviour();
        }
        else
        {
            //TODO: Game over!
        }
    }


    public void nextTurn()
    {
        if (cardInPlay.GetType().IsSubclassOf(typeof(Quest))) {
            BoardManager.DestroyStages();
        }
        BoardManager.DestroyCards();
        BoardManager.DestroyDiscardArea();
        BoardManager.ClearInteractions();
        BoardManager.SetIsFreshTurn(true);
        AddToDiscardDeck(cardInPlay);
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
			Debug.Log ("Current player is: " + players [playerTurn].getName ());
			nextTurn ();
			Debug.Log ("New player is: " + players [playerTurn].getName ());
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
        BoardManager.SetInteractionText("NEW QUEST DRAWN\nWould you like to sponsor this quest?");
		Debug.Log ("The card in play is " + cardInPlay.cardImageName);
        Action action1 = () => {
            ((Quest)cardInPlay).PromptSponsorQuestResponse(true);
        };
        Action action2 = () => {
            ((Quest)cardInPlay).PromptSponsorQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to sponsor quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to sponsor quest.");
	}

	public void SetupQuest(Player player, String text) {
        BoardManager.SetInteractionText(text);

        Action action1 = () => {
            if (((Quest)cardInPlay).isValidQuest()) {
                List<Stage> stages = BoardManager.CollectStageCards();
                ((Quest)cardInPlay).SetupQuestComplete(stages);
            }
            else {
                SetupQuest(player, "INVALID QUEST SELECTIONS.\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages");
            }
        };

        Action action2 = () => {
            ((Quest)cardInPlay).IncrementSponsor();
        };

        if (!BoardManager.QuestPanelsExist()) {
            //Generate panels
            BoardManager.SetupQuestPanels(((Quest)cardInPlay).getNumStages());
        }

        BoardManager.SetInteractionButtons("Complete", "Withdraw Sponsorship", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to setup quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to setup quest.");
	}


	public void PromptAcceptQuest(Player player) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("NEW QUEST DRAWN\nWould you like to participate in this quest?");
        Action action1 = () => {
            ((Quest)cardInPlay).PromptAcceptQuestResponse(true);
        };
        Action action2 = () => {
            ((Quest)cardInPlay).PromptAcceptQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to accept quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to accept quest.");
	}


	public void PromptFoe(Player player, int stageNum) {
        BoardManager.DrawCards(player);
        BoardManager.DisplayStageButton(players);
        BoardManager.SetInteractionText("QUEST STAGE " + (stageNum + 1) + "\nYou are facing a foe. You may place any number of cards, or drop out.");
		Action action1 = () => {
            Debug.Log("Did not dropout");
            TransferFromHandToPlayArea(player);
            Debug.Log("Total battle points in play area is: " + player.getPlayArea().getBattlePoints());
            ((Quest)cardInPlay).getStage(stageNum).PromptFoeResponse(false);
		};
        Action action2 = () => {
            Debug.Log("Dropped out");
            ((Quest)cardInPlay).getStage(stageNum).PromptFoeResponse(true);
        };

		BoardManager.SetInteractionButtons ("Continue", "Drop Out", action1, action2);
	}


    public void TransferFromHandToPlayArea(Player player) {
        BoardManager.GetPlayArea(player);
    }


	public void PromptTest(Player player, int currentBid) {
		//TODO: prompt test
	}

    public void SetInteractionText(String text) {
        BoardManager.SetInteractionText(text);
    }


    public void PromptEnterTournament(Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("NEW TOURNAMENT DRAWN\nWould you like to enter this tournament?");
        Action action1 = () => {
            ((Tournament)cardInPlay).PromptEnterTournamentResponse(true);
        };
        Action action2 = () => {
            ((Tournament)cardInPlay).PromptEnterTournamentResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to enter tournament.");
        Logger.getInstance().info("Prompted " + player.getName() + " to enter tournament.");  
    }


    public void PromptCardSelection(Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("PREPARE FOR BATTLE\nPrepare for the tournament using a combination of weapon, ally and amour cards.");
        Action action = () => {
            ((Tournament)cardInPlay).CardsSelectionResponse();
        };
        BoardManager.SetInteractionButtons("Complete", "", action, null);
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare cards.");
    }


	public void PromptToDiscardWeapon(Player player) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText("Please discard 1 weapon.");
		//Assume player choice is valid for now
		Action action = () => {
			player.RemoveCardsResponse();
			((KingsCallToArms)cardInPlay).PlayerFinishedResponse();
		};
		BoardManager.SetInteractionButtons("Complete", "", action, null);
		Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare weapon cards to discard.");

	}

		
	public void PromptToDiscardFoes(Player player, int numFoes) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText("Please discard " + numFoes +  " Foes.");
		//Assume player choice is valid for now
		Action action = () => {
			player.RemoveCardsResponse();
			((KingsCallToArms)cardInPlay).PlayerFinishedResponse();
		};
		BoardManager.SetInteractionButtons("Complete", "", action, null);
		Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare foe weapon cards to discard.");
	}



    public void PromptCardRemoveSelection(Player player, Action action)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("TOO MANY CARDS\nPlease discard (or play) cards until you have at most 12.");
        BoardManager.SetInteractionButtons("Complete", "", action, null);
        BoardManager.SetupDiscardPanel();
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare cards to discard.");
    }
  

    public void SimpleAlert(Player player, String text) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText(text);

        Action action = () => {
            nextTurn();
        };
        BoardManager.SetInteractionButtons("Continue", "", action, null);
    }

    public void DisplayStageResults(Player player, bool playerEliminated) {
        string passedText = "You passed!";
        if (playerEliminated) {
            passedText = "You were eliminated...";
        }
        Action action = () => {
            ((Quest)cardInPlay).getCurrentStage().EvaluateNextPlayerForFoe(playerEliminated);
        };
        BoardManager.SetIsResolutionOfStage(true);
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("STAGE COMPLETE\n" + passedText);
        BoardManager.SetInteractionButtons("Continue", "", action, null);
        Debug.Log("Displaying results of stage to " + player.getName());
        Logger.getInstance().info("Displaying results of stage to " + player.getName());
    }
}

