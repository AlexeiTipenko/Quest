using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class BoardManagerMediator
{
    
	static BoardManagerMediator instance;
	List<Player> players;
	AdventureDeck adventureDeck;
	StoryDeck storyDeck;
	DiscardDeck adventureDiscard, storyDiscard;
	Story cardInPlay;
	int playerTurn;
    [NonSerialized] PhotonView view;


    [NonSerialized] public GameObject cardPrefab;
    [NonSerialized] public GameObject board;
	public List<CardUI> cards = new List<CardUI>();

    public BoardManagerMediator() {
        if (IsOnlineGame()) {
            view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));   
        }
    }


	public static BoardManagerMediator getInstance() {
		if (instance == null) {
			instance = new BoardManagerMediator ();
		}
		return instance;
	}

	public PhotonView getPhotonView() {
		return view;
	}

	public void initGame (List<Player> players) {
		this.players = players;
        switch (ButtonManager.scenario)
        {
            case "scenario1":
                adventureDeck = (AdventureDeck)Scenario1.getInstance().AdventureDeck();
                storyDeck = (StoryDeck)Scenario1.getInstance().StoryDeck();
                break;
            case "scenario2":
                adventureDeck = (AdventureDeck)Scenario2.getInstance().AdventureDeck();
                storyDeck = (StoryDeck)Scenario2.getInstance().StoryDeck();
                break;
            default:
                adventureDeck = new AdventureDeck();
                storyDeck = new StoryDeck();
                break;
        }
		adventureDiscard = new DiscardDeck ();
		storyDiscard = new DiscardDeck ();
        Logger.getInstance().info("Card decks created");

		foreach (Player player in players) {
            player.DrawCards(12, null);
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
        Debug.Log("Previous player is: " + previousPlayer.getName());
        Logger.getInstance().info("Previous player is: " + previousPlayer.getName());
		int index = -1;
		for (int i = 0; i < players.Count; i++) {
			if (players [i].getName () == previousPlayer.getName ()) {
				index = (i + 1) % players.Count;
			}
		}
		if (index != -1) {
			Debug.Log("returning player");
			Logger.getInstance().info("Returning player in get next player");
			return players [index];
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

    public void DestroyDiscardArea(){
        BoardManager.DestroyDiscardArea();
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


    public void DiscardCard(string cardName) {
        
        foreach (Card card in players[playerTurn].getHand())
        {
            if (card.getCardName() == cardName)
            {
                Debug.Log(card.getCardName() + " removed from " + players[playerTurn].getName() + "'s hand.");
                Logger.getInstance().info(card.getCardName() + " removed from " + players[playerTurn].getName() + "'s hand.");
                players[playerTurn].RemoveCard(card);
                break;
            }
        }
    }

    public void DiscardChosenAlly(string cardName)
    {
        foreach (Player player in players){
            foreach (Card card in player.getPlayArea().getCards())
            {
                if (card.getCardName() == cardName)
                {
                    Debug.Log(card.getCardName() + " removed from " + player.getName() + "'s play area.");
                    Logger.getInstance().info(card.getCardName() + " removed from " + player.getName() + "'s play area.");
                    player.getPlayArea().discardAlly(card.GetType());
                    return;
                }
            }
        }
    }



    public int GetCardsNumHandArea(Player player){
        return BoardManager.GetCardsNumHandArea(player);
    }

    public Card drawAdventureCard() {
        Card card = adventureDeck.drawCard();
        if (adventureDeck.getSize() <= 0) {
            adventureDeck = new AdventureDeck(adventureDiscard);
            adventureDiscard.empty();
        }
        return card;
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

    public bool IsOnlineGame(){

        if (GameObject.Find("DDOL/PunManager") == null)
            return false;

        else
            return true;
    }


    public void startGame()
    {
        Logger.getInstance().info("Game started...");
        playerTurn = 0;

        Logger.getInstance().info("Starting local game");
        Debug.Log("Starting local game");
        playTurn();
    }


    public void playTurn()
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
		Debug.Log ("nextTurn 0");
        if (cardInPlay.GetType().IsSubclassOf(typeof(Quest))) {
            BoardManager.DestroyStages();
        }
        BoardManager.DestroyCards();
        BoardManager.DestroyDiscardArea();
        BoardManager.DestroyMordredDiscardArea();
        //Debug.Log("ENDING TURN FOR PLAYER: " + players[playerTurn]);
        //BoardManager.DestroyMordredButton();
        BoardManager.ClearInteractions();
		Debug.Log ("nextTurn 1");
        BoardManager.SetIsFreshTurn(true);
		Debug.Log ("nextTurn 2");
        AddToDiscardDeck(cardInPlay);
		Debug.Log ("nextTurn 3");
        cardInPlay = null;
		Debug.Log ("nextTurn 4");
        playerTurn = (playerTurn + 1) % players.Count;
		Debug.Log ("Going to next turn for player " + players[playerTurn] );
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
			if (IsOnlineGame ()) {
				view.RPC ("nextTurn", PhotonTargets.Others);
			}
			nextTurn ();
			Debug.Log ("New player is: " + players [playerTurn].getName ());
			break;
		case "discardArea":
			Debug.Log ("CHEAT: Setting up discard area");
			GameObject discardArea = GameObject.Find ("Canvas/TabletopImage/DiscardArea");
			if (discardArea == null) {
				BoardManager.SetupDiscardPanel ();
			} else {
				BoardManager.DestroyDiscardArea ();
				players [playerTurn].GetAndRemoveCards ();
			}
			break;
        }
    }

    //------------------------------------------------------------------------//
    //--------------------------- Visual Functions ---------------------------//
    //------------------------------------------------------------------------//


    public void DrawRank(Player player) {
        BoardManager.DrawRank(player);
    }

    public void PromptSponsorQuest(Quest quest, Player player) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("NEW QUEST DRAWN\nWould you like to sponsor this quest?");
		Debug.Log ("The card in play is " + cardInPlay.cardImageName);
        Action action1 = () => {
			Debug.Log("Action1 for player: " + player.getName());
            if (IsOnlineGame()) {
                view.RPC("PromptSponsorQuestResponse", PhotonTargets.Others, true);
            }
            quest.PromptSponsorQuestResponse(true);
        };
        Action action2 = () => {
            Debug.Log("Action2 for player: " + player.getName());
            if (IsOnlineGame()) {
                view.RPC("PromptSponsorQuestResponse", PhotonTargets.Others, false);
            }
            quest.PromptSponsorQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to sponsor quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to sponsor quest.");
	}

	public void SponsorQuest(Quest quest, Player player, bool firstPrompt) {
        if (firstPrompt) {
            BoardManager.SetInteractionText("PREPARE YOUR QUEST\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain(unique) weapons\n- Battle points must increase between stages");
        } else {
            BoardManager.SetInteractionText("INVALID QUEST SELECTIONS.\n- Each stage contains a foe or a test\n- Maximum one test per quest\n- Foe stages may contain (unique) weapons\n- Battle points must increase between stages");
        }

        Action action1 = () => {
            if (quest.isValidQuest()) {
                List<Stage> stages = BoardManager.CollectStageCards();
                if (IsOnlineGame()) {
                    view.RPC("SponsorQuestComplete", PhotonTargets.Others, PunManager.Serialize(stages));
                }
                quest.SponsorQuestComplete(stages);
            }
            else {
                player.SponsorQuest(quest, false);
            }
        };

        Action action2 = () => {
            if (IsOnlineGame()) {
                view.RPC("IncrementSponsor", PhotonTargets.Others);
            }
            quest.IncrementSponsor();
        };

        if (!BoardManager.QuestPanelsExist()) {
            BoardManager.SetupQuestPanels(quest.getNumStages());
        }

        BoardManager.SetInteractionButtons("Complete", "Withdraw Sponsorship", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to setup quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to setup quest.");
	}


	public void PromptAcceptQuest(Quest quest, Player player) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("NEW QUEST DRAWN\nWould you like to participate in this quest?");
        Action action1 = () => {
            if (IsOnlineGame()) {
                view.RPC("PromptAcceptQuestResponse", PhotonTargets.Others, true);
            }
            quest.PromptAcceptQuestResponse(true);
        };
        Action action2 = () => {
            if (IsOnlineGame()) {
                view.RPC("PromptAcceptQuestResponse", PhotonTargets.Others, false);
            }
            quest.PromptAcceptQuestResponse(false);
        };
        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to accept quest.");
        Logger.getInstance().info("Prompted " + player.getName() + " to accept quest.");
	}


    public void PromptFoe(Quest quest, Player player) {
        Debug.Log("Inside prompt foe mediator, player being prompted is: " + player.getName());
        BoardManager.DrawCards(player);
        Debug.Log("After drawing cards");
        BoardManager.DisplayStageButton(players);
		BoardManager.SetInteractionText("QUEST STAGE " + (quest.getCurrentStage().getStageNum() + 1) + "\nYou are facing a foe. You may place any number of cards, or drop out.");
        Debug.Log("Setup interaction text");
		Action action1 = () => {
			if (quest.ContainsOnlyValidCards(player)) {
				Debug.Log("Did not dropout");
				TransferFromHandToPlayArea(player);
				Debug.Log("Total battle points in play area is: " + player.getPlayArea().getBattlePoints());
				if (IsOnlineGame()) {
					getPhotonView().RPC("PromptFoeResponse", PhotonTargets.Others, false);
				}
				quest.getCurrentStage().PromptFoeResponse(false);
			} else {
				PromptFoe(quest, player);
			}
		};
        Action action2 = () => {
            Debug.Log("Dropped out");
			if (IsOnlineGame()) {
				getPhotonView().RPC("PromptFoeResponse", PhotonTargets.Others, true);
			}
			quest.getCurrentStage().PromptFoeResponse(true);
        };

		BoardManager.SetInteractionButtons ("Continue", "Drop Out", action1, action2);
	}


    public void TransferFromHandToPlayArea(Player player) {
        List<Card> playAreaCards = BoardManager.GetPlayArea(player);
		BoardManager.TransferCards (player, playAreaCards);
    }


	public void PromptEnterTest(Quest quest, Player player, int currentBid) {
        Stage stage = quest.getCurrentStage();
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("Current stage is a test, with a minimum bid of: " + (currentBid + 1) + ". Do you wish to up the bid?");
        BoardManager.SetInteractionBid(currentBid.ToString());
        Action action1 = () => {
            int InteractionBid = 0;
            Int32.TryParse(BoardManager.GetInteractionBid(), out InteractionBid);
            if (InteractionBid > player.getTotalAvailableBids()) {
                Debug.Log("Trying to bid more than they have");
				if (IsOnlineGame()) {
					view.RPC("PromptEnterTest", PhotonTargets.Others, PunManager.Serialize(player), currentBid);
				}
                PromptEnterTest(quest, player, currentBid);
            }
            else if (InteractionBid <= currentBid) {
				if (IsOnlineGame()) {
					view.RPC("PromptEnterTest", PhotonTargets.Others, PunManager.Serialize(player), currentBid);
				}
                PromptEnterTest(quest, player, currentBid);
            }
            else {
				if (IsOnlineGame()) {
					view.RPC("PromptTestResponse", PhotonTargets.Others, false, InteractionBid);
				}
                stage.PromptTestResponse(false, InteractionBid);
            }
        };
        Action action2 = () => {
            stage.PromptTestResponse(true, 0);
        };
        BoardManager.SetInteractionButtons("Continue", "Drop out", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to enter TEST inside stage: " + stage.getStageNum());
        Logger.getInstance().info("Prompting " + player.getName() + " to enter TEST inside stage: " + stage.getStageNum());
	}

    public void PromptDiscardTest(Quest quest, Player player, int currentBid) {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("You are the winner of the Test, and you must discard/play a total of " + currentBid + " bid points.");
        BoardManager.SetupDiscardPanel();
        Action action = () => {
            TransferFromHandToPlayArea(player);
            if (BoardManager.GetSelectedDiscardNames().Count + player.getPlayAreaBid() == currentBid)
            {
				player.GetAndRemoveCards();
				if (IsOnlineGame()) {
					view.RPC("PlayStage", PhotonTargets.Others);
				}
				quest.PlayStage();
            }
            else {
                PromptDiscardTest(quest, player, currentBid);
            }

        };
        BoardManager.SetInteractionButtons("Complete", "", action, null);

        Debug.Log("Prompting " + player.getName() + " to discard TEST inside stage: " + quest.getCurrentStage().getStageNum());
    }

    public void SetInteractionText(String text) {
        BoardManager.SetInteractionText(text);
    }


    public void PromptEnterTournament(Tournament tournament, Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("NEW TOURNAMENT DRAWN\nWould you like to enter this tournament?");
        Action action1 = () => {
			Debug.Log("Action1 (accept) for " + tournament.getCardName() + " for player " + player.getName());
            if (IsOnlineGame()) {
                view.RPC("PromptEnterTournamentResponse", PhotonTargets.Others, true);
            }
            tournament.PromptEnterTournamentResponse(true);

        };

        Action action2 = () => {
			Debug.Log("Action2 (decline) for " + tournament.getCardName() + " for player " + player.getName());
            if (IsOnlineGame()) {
				view.RPC("PromptEnterTournamentResponse", PhotonTargets.Others, false);
            }
            tournament.PromptEnterTournamentResponse(false);
        };

        BoardManager.SetInteractionButtons("Accept", "Decline", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to enter tournament.");
        Logger.getInstance().info("Prompted " + player.getName() + " to enter tournament.");  
    }


    public void PromptCardSelection(Tournament tournament, Player player)
    {
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("PREPARE FOR BATTLE\nPrepare for the tournament using a combination of weapon, ally and amour cards.");
        Action action = () => {
            tournament.CardsSelectionResponse();
        };

        BoardManager.SetInteractionButtons("Complete", "", action, null);
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare cards.");
    }


	public void PromptToDiscardWeapon(Player player) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText("Please discard 1 weapon.");
        BoardManager.SetupDiscardPanel();

		Action action = () => {
            if (IsOnlineGame())
            {
                view.RPC("PlayerDiscardedWeapon", PhotonTargets.Others);
            }
            ((KingsCallToArms)cardInPlay).PlayerDiscardedWeapon();
		};
		

		BoardManager.SetInteractionButtons("Complete", "", action, null);
		Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare weapon cards to discard.");

	}

		
	public void PromptToDiscardFoes(Player player, int numFoes) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText("Please discard " + numFoes +  " Foes.");
        BoardManager.SetupDiscardPanel();
		Action action = () => {
            
            if (IsOnlineGame())
            {
                view.RPC("PlayerDiscardedFoes", PhotonTargets.Others);
            }
			((KingsCallToArms)cardInPlay).PlayerDiscardedFoes();
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
			if (IsOnlineGame()) {
				view.RPC("nextTurn", PhotonTargets.Others);
			}
            nextTurn();
        };
        BoardManager.SetInteractionButtons("Continue", "", action, null);
    }

    public void DisplayStageResults(Stage stage, Player player, bool playerEliminated) {
        string passedText = "You passed!";
        if (playerEliminated) {
            passedText = "You were eliminated...";
        }
        Action action = () => {
			if (IsOnlineGame()) {
				view.RPC("EvaluateNextPlayerForFoe", PhotonTargets.Others, playerEliminated);
			}
            stage.EvaluateNextPlayerForFoe(playerEliminated);
        };
        BoardManager.SetIsResolutionOfStage(true);
        BoardManager.DrawCards(player);
        BoardManager.SetInteractionText("STAGE COMPLETE\n" + passedText);
        BoardManager.SetInteractionButtons("Continue", "", action, null);
        Debug.Log("Displaying results of stage to " + player.getName());
        Logger.getInstance().info("Displaying results of stage to " + player.getName());
    }
}

