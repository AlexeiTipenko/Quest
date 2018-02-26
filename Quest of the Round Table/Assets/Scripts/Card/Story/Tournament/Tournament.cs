using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Tournament : Story
{

    private BoardManagerMediator board;
    protected int bonusShields, playersEntered;
    public Player sponsor, playerToPrompt;
    public List<Player> participatingPlayers;
    public Dictionary<Player, int> pointsDict;
    bool rematch;


    public Tournament(string cardName, int bonusShields) : base(cardName)
    {
		Logger.getInstance ().info ("Starting the Tournament class");
        this.playersEntered = 0;
        this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
        participatingPlayers = new List<Player>();
        pointsDict = new Dictionary<Player, int>();
        rematch = false;
    }


    public int GetBonusShields()
    {
		Logger.getInstance ().trace ("getting bonus shields: " + bonusShields);
        return bonusShields;
    }


    public override void startBehaviour()
    {
		Logger.getInstance().info("Started Tournament behaviour.");
        sponsor = owner;
        playerToPrompt = sponsor;
        board.PromptEnterTournament(sponsor);
    }



    public void PromptEnterTournamentResponse(bool tournamentAccepted)
    {
		Logger.getInstance().debug("Started PromptEnterTournamentResponse behaviour, tournamentAccepted is " + tournamentAccepted);

        if (tournamentAccepted)
        {
            playersEntered++;
            participatingPlayers.Add(playerToPrompt);

            if ((playerToPrompt.getHand().Count() + 1) > 12)
            {
				Logger.getInstance().trace("Dealing One Card to Player");
                Action action = () => {
                    PromptNextPlayer();
                };

                board.dealOneCardToPlayer(playerToPrompt, action);
            }

            else
            {
				Logger.getInstance().trace("Dealing a card to player and prompt the next player");
                board.dealCardsToPlayer(playerToPrompt, 1);
                PromptNextPlayer();
            }
        }

		else {
			Logger.getInstance().trace("Else: prompting next player");
            PromptNextPlayer();
        }
    }

    public void PromptNextPlayer(){
		Logger.getInstance().debug("prompting next player");

        playerToPrompt = board.getNextPlayer(playerToPrompt);

        if (playerToPrompt != sponsor)
            board.PromptEnterTournament(playerToPrompt);

        else
            NumParticipantsAction();
    }


    private void NumParticipantsAction()
    {
        if (participatingPlayers.Count() == 0)
        {
			Logger.getInstance().debug("No participants in tournament.");
            board.nextTurn();
        }

        else if (participatingPlayers.Count() == 1) 
        {
			Logger.getInstance().debug("TOURNAMENT DEFAULT WINNER: " + participatingPlayers[0].getName());
            participatingPlayers[0].incrementShields(1);
            board.nextTurn();
        }

        else
            board.PromptCardSelection(playerToPrompt);
    }



    public void CardsSelectionResponse() {

        List<Card> chosenCards = board.GetSelectedCards(playerToPrompt);
        bool cardsValid = ValidateChosenCards(chosenCards);

        if (!cardsValid)
        {
			Logger.getInstance().debug("Card selection invalid.");
            board.ReturnCardsToPlayer();
            board.PromptCardSelection(playerToPrompt);
        }

        else
        {
            foreach(Card card in chosenCards)
            {
                playerToPrompt.RemoveCard(card);
                playerToPrompt.getPlayArea().addCard(card);
            }

			Logger.getInstance().debug("All cards valid.");
            AddPlayerBattlePoints(chosenCards);

			Logger.getInstance().debug("Player finished turn.");
            playerToPrompt = GetNextPlayer(playerToPrompt);

            if (playerToPrompt == sponsor)
                TournamentRoundComplete();

            else
                board.PromptCardSelection(playerToPrompt);
        }
    }


    public void AddPlayerBattlePoints(List<Card> chosenCards){
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


    public bool ValidateChosenCards(List<Card> chosenCards){
		Logger.getInstance ().debug ("Validating chosen cards");
        bool cardsValid = true;
        
        if (chosenCards.GroupBy(c => c.getCardName()).Any(g => g.Count() > 1))
            cardsValid = false;

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


    public void TournamentRoundComplete() {
		Logger.getInstance ().debug ("Tournament round is complete");
        IEnumerable<Player> tempCollection = from p in pointsDict 
                where p.Value == pointsDict.Max(v => v.Value) select p.Key;
        
        List<Player> winnerList = tempCollection.ToList();

        if (winnerList.Count() == 1) 
        {
			Logger.getInstance ().trace("TOURNAMENT WINNER: " + winnerList[0].getName());
            winnerList[0].incrementShields(playersEntered);
            DiscardCards();
            board.nextTurn();
        }

        else if (winnerList.Count() > 1) 
        {
			Logger.getInstance ().trace("MORE THAN ONE PLAYER WON!");

            if (rematch == false)
            {
				Logger.getInstance ().trace("ROUND 2");
                participatingPlayers = winnerList;
                rematch = true;
                pointsDict.Clear();
                sponsor = participatingPlayers[0];
                playerToPrompt = sponsor;
                //board.PromptEnterTournament(playerToPrompt);
                board.PromptCardSelection(playerToPrompt);
            }

            else{
				Logger.getInstance ().trace("ROUND 3 AND END OF TOURNAMENT.");
                foreach(Player player in winnerList){
                    player.incrementShields(playersEntered);
                }
                DiscardCards();
                board.nextTurn();
            }
        }
        //winners.Dump();
    }

    private void DiscardCards() {
		Logger.getInstance ().debug ("Discarding all cards...");
        //NOT WORKING IN UI
        foreach(Player player in board.getPlayers()){
            player.getPlayArea().discardWeapons();
            player.getPlayArea().discardAmours();
            //Not allies
        }
    }


    public Player GetNextPlayer(Player previousPlayer)
    {
		Logger.getInstance ().debug ("Getting next player...");
        int index = participatingPlayers.IndexOf(previousPlayer);
        if (index != -1)
        {
            if (index < (participatingPlayers.Count - 1))
                return participatingPlayers[index + 1];

            return participatingPlayers[0];
        }
		Logger.getInstance ().trace("There is no next player");
        return null;
    }
}
