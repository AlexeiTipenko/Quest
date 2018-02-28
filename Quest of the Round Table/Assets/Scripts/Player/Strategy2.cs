using UnityEngine;
using System.Collections.Generic;

public class Strategy2 : Strategy
{
    public Strategy2() : base (40) {
        
    }

    public override void DiscardAfterWinningTest()
    {
        throw new System.NotImplementedException();
    }

    public override void DoIParticipateInQuest()
    {
        throw new System.NotImplementedException();
    }

    public override void DoIParticipateInTournament()
    {
        throw new System.NotImplementedException();
    }

    public override bool DoISponsorAQuest()
    {
        if (SomeoneElseCanWinOrEvolveWithQuest()) {
            return false;
        }
        else if (SufficientCardsToSponsorQuest()) {
            return true;
        }
        return false;
    }

    public override void NextBid()
    {
        throw new System.NotImplementedException();
    }

    public override void SponsorQuest()
    {
        Debug.Log(strategyOwner.getName() + " is preparing to sponsor the quest.");
        List<Stage> stages = new List<Stage>();
        Quest quest = (Quest)board.getCardInPlay();
        List<Card> cards = strategyOwner.getHand();

        Stage finalStage = null;
        Stage testStage = null;
        List<Stage> otherStages = new List<Stage>();
        int initializedStages = 0, numTestStages = 0;

        Card stageCard = null;
        List<Card> weapons = new List<Card>();
        foreach (Card card in cards) {
            if (card.GetType().IsSubclassOf(typeof(Foe))) {
                if (stageCard == null || ((Foe)card).getBattlePoints() > ((Foe)stageCard).getBattlePoints()) {
                    stageCard = card;
                }
            }
        }
        Debug.Log("Final stage foe: " + stageCard.getCardName());
        while (((Foe)stageCard).getBattlePoints() + GetTotalBattlePoints(weapons) < minimumFinalStageBattlePoints) {
            weapons.Add(GetBestUniqueWeapon(cards, weapons));
        }
        Debug.Log("Final stage weapons:");
        foreach (Weapon weapon in weapons) {
            Debug.Log(weapon.getCardName());
        }
        finalStage = InitializeStage(stageCard, weapons, quest.getNumStages() - 1);
        initializedStages++;
        Debug.Log("Initialized stages: " + initializedStages);

        if (ContainsTest(cards)) {
            Debug.Log(strategyOwner.getName() + " has a test in their hand.");
            foreach (Card card in cards) {
                if (card.GetType().IsSubclassOf(typeof(Test))) {
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
        while (initializedStages < quest.getNumStages()) {
            stageCard = GetWeakestFoe(cards, previousStageCard);
            int stageNum = initializedStages - (numTestStages + 1);
            Debug.Log("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
            otherStages.Add(InitializeStage(stageCard, null, stageNum));
            initializedStages++;
            previousStageCard = stageCard;
        }

        foreach (Stage stage in otherStages) {
            stages.Add(stage);
        }
        if (testStage != null) {
            stages.Add(testStage);
        }
        stages.Add(finalStage);
        quest.SetupQuestComplete(stages);
    }

    Foe GetWeakestFoe(List<Card> cards, Card previousStageCard) {
        Foe weakestFoe = null;
        foreach (Card card in cards) {
            if (card.GetType().IsSubclassOf(typeof(Foe))) {
                if (weakestFoe == null || ((Foe)card).getBattlePoints() < weakestFoe.getBattlePoints()) {
                    if (previousStageCard == null || ((Foe)previousStageCard).getBattlePoints() < ((Foe)card).getBattlePoints()) {
                        weakestFoe = (Foe)card;
                    }
                }
            }
        }
        return weakestFoe;
    }
}
