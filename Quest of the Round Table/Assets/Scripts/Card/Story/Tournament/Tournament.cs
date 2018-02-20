using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Tournament : Story {

    private BoardManagerMediator board;
	protected int bonusShields;
	protected int playersEntered;
    Player sponsor, playerToPrompt;
    List<Player> participatingPlayers; //, allPlayers;
    Dictionary<Player, int> pointsDict = new Dictionary<Player, int>();


	public Tournament(string cardName, int bonusShields): base(cardName) {
        
		this.playersEntered = 0;
		this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
        participatingPlayers = new List<Player>();
	}
		

	public int GetBonusShields() {
		return bonusShields;
	}


    public override void startBehaviour()
    {
        Debug.Log("Tournament behaviour started");

        sponsor = owner;
        playerToPrompt = sponsor;
        board.PromptEnterTournament(sponsor);
    }



    public void PromptEnterTournamentResponse(bool tournamentAccepted)
    {
        if (tournamentAccepted){
            playersEntered++;
            participatingPlayers.Add(playerToPrompt);
        }
                              
        playerToPrompt = board.getNextPlayer(playerToPrompt);

        if (playerToPrompt != sponsor)
            board.PromptEnterTournament(playerToPrompt);
        
        else
            board.PromptCardSelection(playerToPrompt);
    }



    public void CardsSelectionResponse() {

        List<Card> chosenCards = board.GetSelectedCards(playerToPrompt);
        bool cardsValid = ValidateChosenCards(chosenCards);

        if (!cardsValid)
        {
            Debug.Log("Card selection invalid.");
            board.ReturnCardsToPlayer();
            board.PromptCardSelection(playerToPrompt);
        }

        else
        {
            Debug.Log("All cards valid.");
            AddPlayerBattlePoints(chosenCards);

            Debug.Log("Player finished turn.");
            playerToPrompt = GetNextPlayer(playerToPrompt);

            if (playerToPrompt == sponsor)
                TournamentComplete();

            else
                board.PromptCardSelection(playerToPrompt);
        }
    }


    private void AddPlayerBattlePoints(List<Card> chosenCards){
        int pointsTotal = 0;
        foreach (Card card in chosenCards)
        {
            if (card.GetType().IsSubclassOf(typeof(Adventure)))
            {
                pointsTotal += ((Adventure)card).getBattlePoints();
            }
        }
        pointsDict.Add(playerToPrompt, pointsTotal);
    }

    /*
    public void compare() {
        List<int> battlePointList = new List<int>();
        foreach (Player player in participatingPlayers) {
            battlePointList.Add(player.getBattlePoints());
        }

    }
    */


    private bool ValidateChosenCards(List<Card> chosenCards){

        bool cardsValid = true;
        
        if (chosenCards.GroupBy(c => c.getCardName()).Any(g => g.Count() > 1))
            cardsValid = false;

        //validate chosen cards
        foreach (Card card in chosenCards)
        {
            if (!(card.GetType().IsSubclassOf(typeof(Weapon))) &&
                !(card.GetType().IsSubclassOf(typeof(Ally))) &&
                !(card.GetType().IsSubclassOf(typeof(Amour))))
            {

                cardsValid = false;
            }
        }
        return cardsValid;
    }


    private void TournamentComplete() {
        Debug.Log("Tournament complete!");

        //Check who has the most battle points
        //Award them the proper amount of shields plus bonus shields


        IEnumerable<Player> tempCollection = from p in pointsDict where p.Value == pointsDict.Max(v => v.Value) select p.Key;
        List<Player> winnerList = tempCollection.ToList();

        if (winnerList.Count() == 1) {
            Debug.Log("TOURNEMENT WINNER: " + winnerList[0].getName());
            winnerList[0].incrementShields(playersEntered);
        }

        else if (winnerList.Count() > 1) {
            Debug.Log("MORE THAN ONE PLAYER WON!");
            participatingPlayers = winnerList;
            sponsor = participatingPlayers[0];
            playerToPrompt = sponsor;
            board.PromptEnterTournament(playerToPrompt);
        }

        //winners.Dump();

        foreach (Player player in board.getPlayers())
        {
            player.getPlayArea().discardAmours();
        }

        board.nextTurn();
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
