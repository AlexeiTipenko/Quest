using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


[Serializable]
public class Strategy1 : AbstractAI
{
    public Strategy1() : base (50, 20) {
        
    }

    public override void DiscardAfterWinningTest(int currentBid, Quest quest)
    {
        throw new System.NotImplementedException();
    }

    public override bool DoIParticipateInQuest()
    {
        throw new System.NotImplementedException();
    }

    public override bool DoIParticipateInTournament()
    {
		if (SomeoneElseCanWinOrEvolveWithTournament(board.getPlayers())) 
		{
			return true;
		}
        return false;
    }

    public override bool DoISponsorAQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " AI 1 is preparing to sponsor the quest");
        Debug.Log(strategyOwner.getName() + " AI 1 is preparing to sponsor the quest.");

        if (SomeoneElseCanWinOrEvolveWithQuest())
        {
            return false;
        }
        else if (SufficientCardsToSponsorQuest())
        {
            return true;
        }
        return false;
    }

    public override void NextBid(int currentBid, Stage stage)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    public override void SponsorQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " AI 1 is preparing the quest");
        Debug.Log(strategyOwner.getName() + " AI 1 is preparing the quest.");

        List<Stage> stages = new List<Stage>();
        Quest quest = (Quest)board.getCardInPlay();
        List<Card> cards = strategyOwner.getHand();

        Stage finalStage = null;
        Stage testStage = null;
        List<Stage> otherStages = new List<Stage>();
        int initializedStages = 0, numTestStages = 0;

        Card stageCard = null;
        List<Card> weapons = new List<Card>();
        foreach (Card card in cards)
        {
            if (card.GetType().IsSubclassOf(typeof(Foe)))
            {
                if (stageCard == null || ((Foe)card).getBattlePoints() > ((Foe)stageCard).getBattlePoints())
                {
                    stageCard = card;
                }
            }
        }
        Logger.getInstance().info("Final stage foe: " + stageCard.getCardName());
        Debug.Log("Final stage foe: " + stageCard.getCardName());
        while (((Foe)stageCard).getBattlePoints() + GetTotalBattlePoints(weapons) < minimumFinalStageBattlePoints)
        {
            weapons.Add(GetBestUniqueWeapon(cards, weapons));
        }

        Logger.getInstance().info("Final stage weapons: " + stageCard.getCardName());
        Debug.Log("Final stage weapons:");
        foreach (Weapon weapon in weapons)
        {
            Logger.getInstance().info(weapon.getCardName());
            Debug.Log(weapon.getCardName());
        }
        finalStage = InitializeStage(stageCard, weapons, quest.getNumStages() - 1);
        initializedStages++;
        Logger.getInstance().info("Initialized stages " + initializedStages);
        Debug.Log("Initialized stages: " + initializedStages);

        if (ContainsTest(cards))
        {
            Debug.Log(strategyOwner.getName() + " has a test in their hand.");
            foreach (Card card in cards)
            {
                if (card.GetType().IsSubclassOf(typeof(Test)))
                {
                    stageCard = card;
                    break;
                }
            }
            testStage = InitializeStage(stageCard, null, quest.getNumStages() - 2);
            initializedStages++;
            numTestStages++;
            Debug.Log("Initialized stages: " + initializedStages);
        }

        Card previousStageCard = null;
        while (initializedStages < quest.getNumStages())
        {
            stageCard = strategyOwner.GetWeakestFoe(cards, previousStageCard);
            int stageNum = initializedStages - (numTestStages + 1);
            Logger.getInstance().info("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
            Debug.Log("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
            otherStages.Add(InitializeStage(stageCard, null, stageNum));
            initializedStages++;
            previousStageCard = stageCard;
        }

        foreach (Stage stage in otherStages)
        {
            stages.Add(stage);
        }
        if (testStage != null)
        {
            stages.Add(testStage);
        }
        stages.Add(finalStage);
        quest.SponsorQuestComplete(stages);
    }

    protected override bool CanPlayCardForStage(Card card, List<Card> participationList)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayFoeStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayTestStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    public override List<Card> ParticipateTournament() {
		//throw new System.NotImplementedException();
		Tournament tournament = (Tournament)board.getCardInPlay();
		List<Card> hand = strategyOwner.getHand();
		List<Card> sortedList = SortBattlePointsCards (hand);
		List<Card> participationList = new List<Card> ();
		if (SomeoneElseCanWinOrEvolveWithTournament (tournament.participatingPlayers)) {
			//play strongest possible hand (includes allies, amours and weapons)
			foreach (Card card in sortedList) {
				if (((card.GetType () == typeof(Amour) || card.GetType().IsSubclassOf(typeof(Weapon))) && !participationList.Contains (card)) 
					|| card.GetType().IsSubclassOf(typeof(Ally)))
				{
					participationList.Add (card);
				} 
			}
		} else {
			//Else: I play only weapons I have two or more instances of
			List<Card> weaponList = new List<Card>();
			foreach (Card card in sortedList) {
				if (card.GetType ().IsSubclassOf (typeof(Weapon))) {
					weaponList.Add (card);
				}
			}
			Dictionary<Card, int> weaponCountMap = weaponList.GroupBy( i => i ).ToDictionary(t => t.Key, t=> t.Count());

			foreach (Card card in weaponCountMap.Keys) {
				if (weaponCountMap [card] > 1) {
					participationList.Add (card);
				}
			}
		}
		return participationList;
    }
}
