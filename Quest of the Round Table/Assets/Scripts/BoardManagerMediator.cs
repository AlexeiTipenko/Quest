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
		case "scenario3":
			adventureDeck = (AdventureDeck)Scenario3.getInstance ().AdventureDeck ();
			storyDeck = (StoryDeck)Scenario3.getInstance ().StoryDeck ();
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

    public List<Adventure> GetSelectedCards(Player player) {
        
        List<string> cardNames = BoardManager.GetSelectedCardNames();
        List<Adventure> cardList = new List<Adventure>();

        foreach(string name in cardNames) {
            foreach (Adventure card in player.GetHand()) {
                if (card.GetCardName() == name) {
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

    public List<Adventure> GetDiscardedCards(Player player)
    {

        List<string> cardNames = BoardManager.GetSelectedDiscardNames();
        List<Adventure> cardList = new List<Adventure>();

        foreach (string name in cardNames)
        {
            foreach (Adventure card in player.GetHand())
            {
                if (card.GetCardName() == name)
                {
                    cardList.Add(card);
                    break;
                }
            }
        }
        return cardList;
    }



	public void DiscardCard(string cardName, Player player) {
        foreach (Adventure card in player.GetHand())
        {
            if (card.GetCardName() == cardName)
            {
                Debug.Log(card.GetCardName() + " removed from " + player.getName() + "'s hand.");
                Logger.getInstance().info(card.GetCardName() + " removed from " + player.getName() + "'s hand.");
                player.RemoveCard(card);
                break;
            }
        }
    }

    public void DiscardChosenAlly(string cardName)
    {
        foreach (Player player in players){
            foreach (Card card in player.getPlayArea().getCards())
            {
                if (card.GetCardName() == cardName)
                {
                    Debug.Log(card.GetCardName() + " removed from " + player.getName() + "'s play area.");
                    Logger.getInstance().info(card.GetCardName() + " removed from " + player.getName() + "'s play area.");
                    player.getPlayArea().DiscardChosenAlly(card.GetType());
                    return;
                }
            }
        }
    }



    public int GetCardsNumHandArea(Player player){
        return BoardManager.GetCardsNumHandArea(player);
    }

    public Adventure drawAdventureCard() {
		Adventure card = (Adventure)adventureDeck.drawCard();
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
		if (card.IsStory()) {
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
            Logger.getInstance().info("Drew card: " + cardInPlay.GetCardName());
            Debug.Log("Drew card: " + cardInPlay.GetCardName());
            cardInPlay.startBehaviour();
        }
        else
        {
            //TODO: Game over!
        }
    }


    public void nextTurn()
    {
		if (cardInPlay.IsQuest()) {
            BoardManager.DestroyStages();
        }
        BoardManager.DestroyCards();
        BoardManager.DestroyDiscardArea();
        BoardManager.DestroyMordredDiscardArea();
        BoardManager.ClearInteractions();
        BoardManager.SetIsFreshTurn(true);
        AddToDiscardDeck(cardInPlay);
        cardInPlay = null;
        playerTurn = (playerTurn + 1) % players.Count;
		Debug.Log ("Going to next turn for player " + players[playerTurn] );
        playTurn();
    }

    private bool gameOver()
    {
        foreach (Player player in players)
        {
            if (player.getRank().GetCardName() == "Knight of the Round Table")
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
		BoardManager.SetInteractionText(Localization.PromptSponsorQuest(player));
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
			BoardManager.SetInteractionText(Localization.SponsorQuest(player, true));
        } else {
			BoardManager.SetInteractionText(Localization.SponsorQuest(player, false));
        }

        Action action1 = () => {
            if (quest.IsValidQuest()) {
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
		BoardManager.SetInteractionText(Localization.PromptAcceptQuest(player));
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
        BoardManager.DisplayStageButton(players);
		BoardManager.SetInteractionText (Localization.PromptFoe (player, quest.getCurrentStage ().getStageNum () + 1));
        Debug.Log("Setup interaction text");
		Action action1 = () => {
			if (quest.ContainsOnlyValidCards(player)) {
				Debug.Log("Did not dropout");
				TransferFromHandToPlayArea(player);
				Debug.Log("Total battle points in play area is: " + player.getPlayArea().GetBattlePoints());
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
        List<Adventure> playAreaCards = BoardManager.GetPlayArea(player);
		BoardManager.TransferCards (player, playAreaCards);
    }


	public void PromptEnterTest(Quest quest, Player player, int currentBid) {
        Stage stage = quest.getCurrentStage();
        BoardManager.DrawCards(player);
		BoardManager.SetInteractionText (Localization.PromptTest (player, stage.getStageNum (), currentBid + 1));
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
			if (IsOnlineGame()) {
				view.RPC("PromptTestResponse", PhotonTargets.Others, true, 0);
			}
            stage.PromptTestResponse(true, 0);
        };
        BoardManager.SetInteractionButtons("Continue", "Drop out", action1, action2);
        Debug.Log("Prompting " + player.getName() + " to enter TEST inside stage: " + stage.getStageNum());
        Logger.getInstance().info("Prompting " + player.getName() + " to enter TEST inside stage: " + stage.getStageNum());
	}

    public void PromptDiscardTest(Quest quest, Player player, int currentBid) {
        BoardManager.DrawCards(player);
		BoardManager.SetInteractionText (Localization.PromptDiscardTest (player, quest.getCurrentStage ().getStageNum (), currentBid));
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
		BoardManager.SetInteractionText(Localization.PromptEnterTournament(player));
        Action action1 = () => {
			Debug.Log("Action1 (accept) for " + tournament.GetCardName() + " for player " + player.getName());
            Logger.getInstance().info("Action1 (accept) for " + tournament.GetCardName() + " for player " + player.getName());
            if (IsOnlineGame()) {
                view.RPC("PromptEnterTournamentResponse", PhotonTargets.Others, true);
            }
            tournament.PromptEnterTournamentResponse(true);

        };

        Action action2 = () => {
			Debug.Log("Action2 (decline) for " + tournament.GetCardName() + " for player " + player.getName());
            Logger.getInstance().info("Action2 (decline) for " + tournament.GetCardName() + " for player " + player.getName());
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
		BoardManager.SetInteractionText(Localization.PrepareTournament(player));
        Action action = () => {
			List<Adventure> chosenCards = GetSelectedCards(player);
			if (tournament.ValidateChosenCards(chosenCards)) {
				if (IsOnlineGame()) {
					view.RPC("CardsSelectionResponse", PhotonTargets.Others, PunManager.Serialize(chosenCards));
				}
				tournament.CardsSelectionResponse(chosenCards);
			} else {
				PromptCardSelection(tournament, player);
			}
        };

        BoardManager.SetInteractionButtons("Complete", "", action, null);
        Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare cards.");
    }

	public void DisplayTournamentResults (Tournament tournament, Player player, bool playerEliminated) {
		BoardManager.DrawCards (player);
		BoardManager.SetInteractionText (Localization.DisplayTournamentResults(player, playerEliminated));
		Action action = () => {
			if (IsOnlineGame()) {
				view.RPC("DisplayTournamentResults", PhotonTargets.Others);
			}
			tournament.DisplayTournamentResultsResponse ();
		};
		BoardManager.SetInteractionButtons ("Continue", "", action, null);
		Debug.Log("Displaying tournament results to " + player.getName());
		Logger.getInstance().info("Displaying tournament results to " + player.getName());
	}


	public void PromptToDiscardWeapon(KingsCallToArms card, Player player) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText(Localization.PromptToDiscardWeapon(player));
        BoardManager.SetupDiscardPanel();

		Action action = () => {
            if (IsOnlineGame())
            {
                view.RPC("PlayerDiscardedWeapon", PhotonTargets.Others);
            }
            card.PlayerDiscardedWeapon();
		};
		

		BoardManager.SetInteractionButtons("Complete", "", action, null);
		Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare weapon cards to discard.");

	}


	public void PromptToDiscardFoes(KingsCallToArms card, Player player, int numFoes) 
	{
		BoardManager.DrawCards(player);
		BoardManager.SetInteractionText (Localization.PromptToDiscardFoes(player, numFoes));
        BoardManager.SetupDiscardPanel();
		Action action = () => {
            
            if (IsOnlineGame())
            {
                view.RPC("PlayerDiscardedFoes", PhotonTargets.Others);
            }
			card.PlayerDiscardedFoes();
		};
		BoardManager.SetInteractionButtons("Complete", "", action, null);
		Debug.Log("Prompting " + player.getName() + " to prepare cards.");
        Logger.getInstance().info("Prompted " + player.getName() + " to prepare foe weapon cards to discard.");
	}



    public void PromptCardRemoveSelection(Player player, Action action)
    {
        BoardManager.DrawCards(player);
		BoardManager.SetInteractionText(Localization.PromptCardRemoveSelection(player));
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
        Action action = () => {
			if (IsOnlineGame()) {
				view.RPC("EvaluateNextPlayerForFoe", PhotonTargets.Others, playerEliminated);
			}
            stage.EvaluateNextPlayerForFoe(playerEliminated);
        };
        BoardManager.SetIsResolutionOfStage(true);
        BoardManager.DrawCards(player);
		BoardManager.SetInteractionText("STAGE COMPLETE\n" + Localization.DisplayStageResults(player, playerEliminated));
        BoardManager.SetInteractionButtons("Continue", "", action, null);
        Debug.Log("Displaying results of stage to " + player.getName());
        Logger.getInstance().info("Displaying results of stage to " + player.getName());
    }
}

