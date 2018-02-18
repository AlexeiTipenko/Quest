using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tournament : Story {

    private BoardManagerMediator board;
	protected int bonusShields;
	protected int playersEntered;
    Player sponsor, playerToPrompt;
    List<Player> participatingPlayers;


	public Tournament(string cardName, int bonusShields): base(cardName) {
        
		this.playersEntered = 0;
		this.bonusShields = bonusShields;
        board = BoardManagerMediator.getInstance();
	}
		

	public int GetBonusShields() {
		return bonusShields;
	}


    public override void startBehaviour()
    {
        Debug.Log("Tournament behaviour started");

        sponsor = owner;
        board.promptSponsorQuest(sponsor);
    }


    public void PromptEnterTournamentResponse(bool sponsorAccepted)
    {
        if (sponsorAccepted)
        {
            playersEntered++;
            board.setupQuest(sponsor);
        }
        else
        {
            sponsor = board.getNextPlayer(sponsor);

            if (sponsor == owner)
            {
                //TODO: discard();
            }

            else
                board.promptSponsorQuest(sponsor);
        }
    }



    public void PromptAcceptTournamentResponse(bool tournamentAccepted)
    {
        if (tournamentAccepted)
            participatingPlayers.Add(playerToPrompt);

        playerToPrompt = board.getNextPlayer(playerToPrompt);

        if (playerToPrompt != sponsor)
            board.promptAcceptTournament(playerToPrompt);
        
        else
            board.promptCardSelection(playerToPrompt);

    }



    public void CardsSelectionResponse() {
        //Get cards from player area (need to connect PlayerPlayArea to CardUI next)
        //Add up battle points for current player

        sponsor = board.getNextPlayer(sponsor);

        if (sponsor == owner)
            TournamentComplete();

        else
            board.promptCardSelection(playerToPrompt);
    }


    private void TournamentComplete() {
        //Processing winner here
    }



}
