using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tournament : Story {

    private BoardManagerMediator board;
	protected int bonusShields;
	protected int playersEntered;
    Player sponsor, playerToPrompt;
    List<Player> participatingPlayers, allPlayers;


	public Tournament(string cardName, int bonusShields): base(cardName) {
        
		this.playersEntered = 0;
		this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
        participatingPlayers = new List<Player>();
        allPlayers = board.getPlayers();
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
                              
        playerToPrompt = GetNextPlayer(playerToPrompt);

        if (playerToPrompt != sponsor)
            board.PromptEnterTournament(playerToPrompt);
        
        else
            board.PromptCardSelection(playerToPrompt);
    }



    public void CardsSelectionResponse() {
        //Get cards from player area (need to connect PlayerPlayArea to CardUI next)
        //Add up battle points for current player

        Debug.Log("Player finished turn.");
        //sponsor = board.getNextPlayer(sponsor);
        playerToPrompt = GetNextPlayer(playerToPrompt);

        if (playerToPrompt == sponsor)
            TournamentComplete();

        else
            board.PromptCardSelection(playerToPrompt);
    }


    private void TournamentComplete() {
        Debug.Log("Tournament complete!");

        //Check who has the most battle points
        //Award them the proper amount of shields plus bonus shields

        foreach (Player player in board.getPlayers())
        {
            player.getPlayArea().discardAmours();
        }

        board.nextTurn();
    }


    public Player GetNextPlayer(Player previousPlayer)
    {
        int index = allPlayers.IndexOf(previousPlayer);
        if (index != -1)
        {
            if (index < (allPlayers.Count - 1))
                return allPlayers[index + 1];

            return allPlayers[0];
        }

        return null;
    }
}
