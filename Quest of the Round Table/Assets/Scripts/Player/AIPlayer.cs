using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AIPlayer : Player {
    
    readonly AbstractAI strategy;

    public AIPlayer(string name, AbstractAI strategy) : base(name) {
        this.strategy = strategy;
        strategy.SetStrategyOwner(this);
    }

	public override void PromptDiscardCards(Action action)
	{
        RemoveRandomCards(hand.Count - 12);
        action.Invoke();
	}

    public override void DiscardCards(Action invalidAction, Action continueAction)
    {
        continueAction.Invoke();
    }

	public override void PromptSponsorQuest(Quest quest)
	{
        if (strategy.DoISponsorAQuest()) {
            quest.PromptSponsorQuestResponse(true);
        } else {
            quest.PromptSponsorQuestResponse(false);
        }
	}

    public override void SponsorQuest(Quest quest, bool firstPrompt)
    {
        strategy.SponsorQuest();
    }

	public override void PromptAcceptQuest(Quest quest)
	{
        if (strategy.DoIParticipateInQuest()) {
            quest.PromptAcceptQuestResponse(true);
        } else {
            quest.PromptAcceptQuestResponse(false);
        }
	}

	public override void PromptFoe(Quest quest)
	{
        strategy.PlayQuestStage(quest.getCurrentStage());
	}

	public override void DisplayStageResults(Stage stage, bool playerEliminated)
	{
        stage.EvaluateNextPlayerForFoe(playerEliminated);
	}

    public override void PromptTest(Quest quest, int currentBid)
    {
        Logger.getInstance().debug("Player is AI");
        strategy.NextBid(currentBid, quest.getCurrentStage());
    }

	public override void PromptDiscardTest(Quest quest, int currentBid)
	{
        Logger.getInstance().debug("Player is AI, AI has won test, discarding cards");
        strategy.DiscardAfterWinningTest(currentBid, quest);
        quest.PlayStage();
	}

	public override void PromptEnterTournament(Tournament tournament)
	{
        if (strategy.DoIParticipateInTournament()) {
            tournament.PromptEnterTournamentResponse(true);
        } else {
            tournament.PromptEnterTournamentResponse(false);
        }
	}

	public override void PromptTournament(Tournament tournament)
	{
        tournament.CardsSelectionResponse();
	}

	public AbstractAI GetStrategy()
    {
        return strategy;
    }

}
