using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HumanPlayer : Player {

    public HumanPlayer(string name) : base (name) {
    }

	public override void PromptDiscardCards(Action action)
	{
        board.PromptCardRemoveSelection(this, action);
	}

    public override void DiscardCards(Action invalidAction, Action continueAction)
    {
        Debug.Log("INSIDE DISCARD CARDS");
        board.TransferFromHandToPlayArea(this);
		GetAndRemoveCards ();
        if (hand.Count > 12)
        {
            Debug.Log("INSIDE DISCARD CARDS, GREATER THAN 12");
			discarded = true;
            PromptDiscardCards(invalidAction);
        }
        else
        {
            continueAction.Invoke();
        }
    }

	public override void PromptSponsorQuest(Quest quest)
	{
        board.PromptSponsorQuest(quest, this);
	}

    public override void SponsorQuest(Quest quest, bool firstPrompt)
    {
        board.SponsorQuest(quest, this, firstPrompt);
    }

	public override void PromptAcceptQuest(Quest quest)
	{
        board.PromptAcceptQuest(quest, this);
	}

	public override void PromptFoe(Quest quest)
	{
        board.PromptFoe(quest, this);
	}

	public override void DisplayStageResults(Stage stage, bool playerEliminated)
	{
        board.DisplayStageResults(stage, this, playerEliminated);
	}

	public override void PromptTest(Quest quest, int currentBid)
	{
        board.PromptEnterTest(quest, this, currentBid);
	}

	public override void PromptDiscardTest(Quest quest, int currentBid)
	{
        board.PromptDiscardTest(quest, this, currentBid);
	}

	public override void PromptEnterTournament(Tournament tournament)
	{
        board.PromptEnterTournament(tournament, this);
	}

	public override void PromptTournament(Tournament tournament)
	{
        board.PromptCardSelection(tournament, this);
	}
}
