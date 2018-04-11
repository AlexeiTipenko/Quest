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

    public int GetTotalAvailableFoeBids()
    {
        int availableBids = 0;
        foreach (Card card in hand)
        {
            if (card.GetType().IsSubclassOf(typeof(Foe)))
            {
                availableBids += 1;
            }
        }
        return availableBids;
    }

    public int getTotalAvailableFoeandDuplicateBids()
    {
        int availableBids = 0;
        Dictionary<String, int> cardDictionary = new Dictionary<String, int>();
        foreach (Card card in hand)
        {
            if (!cardDictionary.ContainsKey(card.getCardName()) && !card.GetType().IsSubclassOf(typeof(Foe)))
            {
                Debug.Log("Inserting into Dictionary: " + card.getCardName());
                cardDictionary.Add(card.getCardName(), 1);
            }
            else if (!card.GetType().IsSubclassOf(typeof(Foe)))
            {
                cardDictionary[card.getCardName()]++;
            }
        }
        foreach (KeyValuePair<String, int> entry in cardDictionary)
        {
            if (entry.Value > 1)
            {
                Debug.Log("Found Duplicate!: " + entry.Value + ", " + entry.Key);
                availableBids += entry.Value - 1;
            }
        }
        Debug.Log("Duplicates are: " + availableBids + " inside Foe and Dups");
        return GetTotalAvailableFoeBids() + availableBids;
    }

    public void RemoveFoeCards()
    {
        List<Card> TempHand = new List<Card>(hand);

        foreach (Card card in TempHand)
        {
            if (card.GetType().IsSubclassOf(typeof(Foe)))
            {
                RemoveCard(card);
            }
        }
    }

    public void RemoveFoeAndDuplicateCards()
    {
        List<String> Seen = new List<String>();
        List<Card> TempHand = new List<Card>(hand);

        RemoveFoeCards();

        foreach (Card card in TempHand)
        {
            if (!Seen.Contains(card.getCardName()))
            {
                Seen.Add(card.getCardName());
            }
            else
            {
                RemoveCard(card);
            }
        }
    }

}
