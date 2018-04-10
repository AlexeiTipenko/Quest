using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public abstract class Tournament : Story
{
    Action action = null;
    private BoardManagerMediator board;
    protected int bonusShields, playersEntered;
    public Player playerToPrompt;
    public List<Player> participatingPlayers;
	List<Player> winnerList;
	int maxPoints;
    bool isLastRound;


    protected Tournament(string cardName, int bonusShields) : base(cardName)
    {
        this.playersEntered = 0;
        this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
        participatingPlayers = new List<Player>();
		winnerList = new List<Player>();
        isLastRound = false;
    }


    public int GetBonusShields()
    {
        return bonusShields;
        //Logger.getInstance().trace("getting bonus shields: " + bonusShields);
    }


    public override void startBehaviour()
    {
		Logger.getInstance().info("Started Tournament behaviour");
        playerToPrompt = owner;
        owner.PromptEnterTournament(this);
    }


    public void PromptEnterTournamentResponse(bool tournamentAccepted)
    {
        if (tournamentAccepted)
        {
            playersEntered++;
            Debug.Log(playerToPrompt.getName() + " has opted to participate in the tournament");
            participatingPlayers.Add(playerToPrompt);

            action = () => {
				Action completeAction = () => {
					Logger.getInstance ().debug ("In Tournament PromptEnterTournamentResponse(), about to RPC promptNextPlayer");
					Debug.Log("In Tournament PromptEnterTournamentResponse(), about to RPC promptNextPlayer");
					if (board.IsOnlineGame()) {
						board.getPhotonView().RPC("PromptNextPlayer", PhotonTargets.Others);
					}
					PromptNextPlayer();
				};
                if (playerToPrompt.getHand().Count > 12) {
                    playerToPrompt.DiscardCards(action, completeAction);
                }
                else {
                    PromptNextPlayer();
                }
            };
            playerToPrompt.DrawCards(1, action);
        }
        else {
			Debug.Log ("Prompting next player...");
            PromptNextPlayer();
        }
    }


    public void PromptNextPlayer(){

        playerToPrompt = board.getNextPlayer(playerToPrompt);

		if (playerToPrompt != owner) {
            Debug.Log("Prompting next person to enter tournament");
			playerToPrompt.PromptEnterTournament (this);
		} else {
			Debug.Log ("NumParticipantsAction...");
			NumParticipantsAction ();
		}
    }


    void NumParticipantsAction()
    {
        if (participatingPlayers.Count == 0) {
            Debug.Log("No participants in tournament");
            Logger.getInstance().info("No participants in tournament");
            board.nextTurn();
        }
        else if (participatingPlayers.Count() == 1) {
            //TODO: show the winner to all relevant players
            Logger.getInstance().info("TOURNAMENT DEFAULT WINNER: " + participatingPlayers[0].getName().ToUpper());
            participatingPlayers[0].incrementShields(1 + bonusShields);
            Logger.getInstance().info("Number of shields: " + participatingPlayers[0].getNumShields());
            board.nextTurn();
        }
        else {
            playerToPrompt.PromptTournament(this);
        }
    }


	public void CardsSelectionResponse(List<Card> chosenCards) {
        foreach (Card card in chosenCards) {
            playerToPrompt.RemoveCard(card);
            playerToPrompt.getPlayArea().addCard(card);
        }

        AddPlayerBattlePoints(chosenCards);

        playerToPrompt = GetNextPlayer(playerToPrompt);

		if (playerToPrompt == owner) {
			TournamentRoundComplete ();
		} else {
			playerToPrompt.PromptTournament (this);
		}
    }


    public void AddPlayerBattlePoints(List<Card> chosenCards) {
        int pointsTotal = 0;
        foreach (Card card in chosenCards)
        {
            if (card.GetType().IsSubclassOf(typeof(Adventure)))
            {
				Debug.Log ("Adding to battle point total");
                pointsTotal += ((Adventure)card).getBattlePoints();
            }
        }
		if (winnerList.Count == 0) {
			winnerList.Add (playerToPrompt);
			maxPoints = pointsTotal;
		} else if (pointsTotal > maxPoints) {
			winnerList.Clear ();
			winnerList.Add (playerToPrompt);
			maxPoints = pointsTotal;
		} else if (pointsTotal == maxPoints){
			winnerList.Add (playerToPrompt);
		}
		Debug.Log ("Total battle points for player " + playerToPrompt.getName() + ": " + pointsTotal);
        Logger.getInstance().info(playerToPrompt.getName() + " has " + pointsTotal + " battle points");
    }


    public bool ValidateChosenCards(List<Card> chosenCards) {
        bool cardsValid = true;
        
        if (chosenCards.GroupBy(c => c.getCardName()).Any(g => g.Count() > 1))
            cardsValid = false;

        foreach (Card card in chosenCards)
        {
            if (!(card.GetType().IsSubclassOf(typeof(Weapon))) &&
                !(card.GetType().IsSubclassOf(typeof(Ally))) &&
                (card.GetType() != typeof(Amour)))
            {

                cardsValid = false;
            }
        }
        return cardsValid;
    }


    public void TournamentRoundComplete() {
		Logger.getInstance ().info ("Tournament round complete");

        if (winnerList.Count() == 1) {
			isLastRound = true;
        }
		CompleteTournament ();
    }

	private void CompleteTournament() {
		Logger.getInstance ().info("Tournament complete, awarding shields");
		if (isLastRound) {
			foreach(Player player in winnerList) {
				player.incrementShields(playersEntered);
			}
		}
		DisplayTournamentResults ();
	}


	private void DisplayTournamentResults() {
		bool playerEliminated = true;
		if (winnerList.Contains(playerToPrompt)) {
			playerEliminated = false;
		}
		playerToPrompt.DisplayTournamentResults (this, playerEliminated);
	}

	public void DisplayTournamentResultsResponse() {
		playerToPrompt = GetNextPlayer (playerToPrompt);
		if (playerToPrompt != owner) {
			DisplayTournamentResults ();
		} else if (!isLastRound) {
			Logger.getInstance ().info("Round 2 of tournament started");
			participatingPlayers = new List<Player>(winnerList);
			isLastRound = true;
			winnerList.Clear();
			owner = participatingPlayers[0];
			playerToPrompt = owner;
			playerToPrompt.PromptTournament(this);
		} else {
			DiscardCards ();
			board.nextTurn ();
		}
	}

    private void DiscardCards() {
        foreach(Player player in board.getPlayers()) {
            player.getPlayArea().discardWeapons();
            player.getPlayArea().discardAmours();
        }
    }


    public Player GetNextPlayer(Player previousPlayer)
    {
        int index = participatingPlayers.IndexOf(previousPlayer);
        if (index != -1)
        {
            if (index < (participatingPlayers.Count - 1))
                return participatingPlayers[index + 1];

            return participatingPlayers[0];
        }
        return null;
    }
}
