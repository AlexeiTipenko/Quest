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
        this.playersEntered = 0;
        this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
        participatingPlayers = new List<Player>();
        pointsDict = new Dictionary<Player, int>();
        rematch = false;
    }


    public int GetBonusShields()
    {
        return bonusShields;
        //Logger.getInstance().trace("getting bonus shields: " + bonusShields);
    }


    public override void startBehaviour()
    {
		Logger.getInstance().info("Started Tournament behaviour");
        sponsor = owner;
        playerToPrompt = sponsor;
        board.PromptEnterTournament(sponsor);
    }


    public void PromptEnterTournamentResponse(bool tournamentAccepted)
    {
        if (tournamentAccepted)
        {
            playersEntered++;
            participatingPlayers.Add(playerToPrompt);

            if ((playerToPrompt.getHand().Count() + 1) > 12)
            {
                Logger.getInstance().trace("Dealing One Card to " + playerToPrompt.getName());
                Action action = () => {
                    board.TransferFromHandToPlayArea(playerToPrompt);
                    playerToPrompt.RemoveCardsResponse();
                    PromptNextPlayer();
                };
                playerToPrompt.giveAction(action);
                board.dealCardsToPlayer(playerToPrompt, 1);
            }

            else
            {
                board.dealCardsToPlayer(playerToPrompt, 1);
                PromptNextPlayer();
            }
        }

        else {

            PromptNextPlayer();
        }
    }


    public void PromptNextPlayer(){

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
			Logger.getInstance().info("No participants in tournament");
            board.nextTurn();
        }

        else if (participatingPlayers.Count() == 1) 
        {
            Logger.getInstance().info("TOURNAMENT DEFAULT WINNER: " + participatingPlayers[0].getName().ToUpper());
            participatingPlayers[0].incrementShields(1);
            Logger.getInstance().info("Number of shields: " + participatingPlayers[0].getNumShields());
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
            Logger.getInstance().warn(playerToPrompt.getName() + "'s card selection INVALID");
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

            Logger.getInstance().info(playerToPrompt.getName() + "'s card selection VALID");
            AddPlayerBattlePoints(chosenCards);

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
        Logger.getInstance().info(playerToPrompt.getName() + " has " + pointsTotal + " battle points");
    }


    public bool ValidateChosenCards(List<Card> chosenCards){
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
        IEnumerable<Player> tempCollection = from p in pointsDict 
                where p.Value == pointsDict.Max(v => v.Value) select p.Key;
        
        List<Player> winnerList = tempCollection.ToList();

        if (winnerList.Count() == 1) 
        {
			Logger.getInstance ().info("Tournament winner: " + winnerList[0].getName());
            winnerList[0].incrementShields(playersEntered);
            DiscardCards();
            board.nextTurn();
        }

        else if (winnerList.Count() > 1) 
        {
			Logger.getInstance ().trace("More than one player has won");

            if (rematch == false)
            {
				Logger.getInstance ().info("Round 2 of tournament started");
                participatingPlayers = winnerList;
                rematch = true;
                pointsDict.Clear();
                sponsor = participatingPlayers[0];
                playerToPrompt = sponsor;
                //board.PromptEnterTournament(playerToPrompt);
                board.PromptCardSelection(playerToPrompt);
            }

            else{
                Logger.getInstance ().info("Round 3 (final) of tournament started");
                foreach(Player player in winnerList){
                    player.incrementShields(playersEntered);
                }
                DiscardCards();
                board.nextTurn();
            }
        }
    }


    private void DiscardCards() {
        foreach(Player player in board.getPlayers()){
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
