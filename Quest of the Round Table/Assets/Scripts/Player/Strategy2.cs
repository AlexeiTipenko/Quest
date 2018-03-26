using System;
using UnityEngine;
using System.Collections.Generic;

public class Strategy2 : AbstractAI
{
    int previousStageBattlePoints = 0;

    public Strategy2() : base (40, 25) {
        
    }

    public override void DiscardAfterWinningTest()
    {
        throw new System.NotImplementedException();
    }

    public override bool DoIParticipateInQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " is deciding to enter the quest.");
        if (IncrementableCardsOverEachStage() && SufficientDiscardableCards()) {
            Logger.getInstance().info(strategyOwner.getName() + " has opted to participate in this quest.");
            Debug.Log(strategyOwner.getName() + " has opted to participate in this quest.");
            return true;
        }
        Debug.Log(strategyOwner.getName() + " has opted to not participate in this quest.");
        Logger.getInstance().info(strategyOwner.getName() + " has opted to not participate in this quest.");
        return false;
    }

    public override bool DoIParticipateInTournament()
    {
        Logger.getInstance().info(strategyOwner.getName() + " has decided to enter the tournament.");
        return true;
    }

    public override bool DoISponsorAQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " deciding to sponsor quest");
        Debug.Log("Prompting " + strategyOwner.getName() + " to sponsor quest");
        if (!SomeoneElseCanWinOrEvolveWithQuest()) {
            Debug.Log("One of two conditions satisfied for AI participation");
            if (SufficientCardsToSponsorQuest()) {
                Debug.Log("Two of two conditions satisfied for AI participation");
                return true;
            }
            return false;
        }
        return false;
    }

    public override void NextBid()
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestStage(Stage stage)
    {
        Logger.getInstance().info(strategyOwner.getName() + " is playing quest stage " + stage.getStageNum());
        Debug.Log(strategyOwner.getName() + " is playing quest stage " + stage.getStageNum());
        if (stage.getStageCard().GetType().IsSubclassOf(typeof(Test))) {
            PlayQuestStage(stage);
        } else {
            PlayFoeStage(stage);
        }
    }

    public override void SponsorQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " is preparing the quest");
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
        Logger.getInstance().info("Final stage foe: " + stageCard.getCardName());
        Debug.Log("Final stage foe: " + stageCard.getCardName());
        while (((Foe)stageCard).getBattlePoints() + GetTotalBattlePoints(weapons) < minimumFinalStageBattlePoints) {
            weapons.Add(GetBestUniqueWeapon(cards, weapons));
        }

        Logger.getInstance().info("Final stage weapons: " + stageCard.getCardName());
        Debug.Log("Final stage weapons:");
        foreach (Weapon weapon in weapons) {
            Logger.getInstance().info(weapon.getCardName());
            Debug.Log(weapon.getCardName());
        }
        finalStage = InitializeStage(stageCard, weapons, quest.getNumStages() - 1);
        initializedStages++;
        Logger.getInstance().info("Initialized stages " + initializedStages);
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
            Logger.getInstance().info("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
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
        quest.SponsorQuestComplete(stages);
    }

    protected override bool CanPlayCardForStage(Card card, List<Card> participationList)
    {
        if (card.GetType() == typeof(Amour)) {
            foreach (Card participationCard in participationList) {
                if (participationCard.GetType() == typeof(Amour)) {
                    return false;
                }
            }
            foreach (Card playAreaCard in strategyOwner.getPlayArea().getCards()) {
                if (playAreaCard.GetType() == typeof(Amour)) {
                    return false;
                }
            }
        }
        else if (card.GetType().IsSubclassOf(typeof(Weapon))) {
            foreach (Card participationCard in participationList) {
                if (participationCard.GetType() == card.GetType()) {
                    return false;
                }
            }
        }
        return true;
    }

    protected override void PlayFoeStage(Stage stage)
    {
        Debug.Log("Stage card type is Foe");
        Quest quest = (Quest)board.getCardInPlay();
        List<Card> cards = strategyOwner.getHand();
        List<Card> sortedList = SortBattlePointsCards(cards);
        List<Card> participationList = new List<Card>();

        if (stage.getStageNum() == quest.getNumStages() - 1) {
            Debug.Log("Final stage! UNLIMITED POWAAAAAAAAAAAAAAAAR");
            foreach (Card card in sortedList) {
                Debug.Log("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.getCardName());
                if (CanPlayCardForStage(card, participationList)) {
                    Debug.Log(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    participationList.Add(card);
                }
            }
        } else {
            Debug.Log("Not the final stage. Play in increments of 10");
            int currentBattlePoints = strategyOwner.getPlayArea().getBattlePoints();
            Debug.Log("Minimum battle points to pass: " + (previousStageBattlePoints + 10));
            foreach (Card card in sortedList) {
                Debug.Log("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.getCardName());
                if (CanPlayCardForStage(card, participationList)) {
                    Debug.Log(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    participationList.Add(card);
                    currentBattlePoints += ((Adventure)card).getBattlePoints();
                    //if (card.GetType() == typeof(Amour)) {
                    //    currentBattlePoints += ((Amour)card).getBattlePoints();
                    //} else if (card.GetType().IsSubclassOf(typeof(Ally))) {
                    //    currentBattlePoints += ((Ally)card).getBattlePoints();
                    //} else {
                    //    currentBattlePoints += ((Weapon)card).getBattlePoints();
                    //}
                    Debug.Log(strategyOwner.getName() + "'s current battle points: " + currentBattlePoints);
                    if (currentBattlePoints >= previousStageBattlePoints + 10) {
                        Debug.Log("Sufficient battle points acquired, moving on with stage");
                        break;
                    }
                }
            }
            if (currentBattlePoints < previousStageBattlePoints + 10) {
                Debug.Log("Whoopsies, somehow the participation condition was violated. Dropping out of quest.");
                stage.PromptFoeResponse(true);
            }
            previousStageBattlePoints = currentBattlePoints;
        }
        foreach (Card card in participationList) {
            Debug.Log("Moving card from " + strategyOwner.getName() + "'s hand to play area: " + card.getCardName());
            strategyOwner.getPlayArea().addCard(card);
            strategyOwner.RemoveCard(card);
        }
        stage.PromptFoeResponse(false);
    }

    protected override void PlayTestStage(Stage stage)
    {
        Debug.Log("Stage card type is Test");
        throw new NotImplementedException();
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

    bool IncrementableCardsOverEachStage() {
        Quest quest = (Quest)board.getCardInPlay();
        List<Card> cards = strategyOwner.getHand();
        List<Card> sortedList = SortBattlePointsCards(cards);
        List<Card> participationList = new List<Card>();

        int previousBattlePoints = 0;
        int permanentBattlePoints = 0;
        int currentBattlePoints = 0;
        for (int i = 0; i < quest.getNumStages(); i++) {
            Debug.Log("Calculating " + strategyOwner.getName() + "'s validity for stage " + i);
            List<Card> tempList = new List<Card>(sortedList);
            previousBattlePoints = currentBattlePoints;
            currentBattlePoints = permanentBattlePoints;
            Debug.Log("Required battle points: " + (previousBattlePoints + 10));

            foreach (Card card in sortedList) {
                Debug.Log("Adding " + card.getCardName() + " to " + strategyOwner.getName() + "'s hypothetical play area");
                currentBattlePoints += ((Adventure)card).getBattlePoints();
                if (card.GetType() == typeof(Amour)) {
                    permanentBattlePoints += ((Amour)card).getBattlePoints();
                    //currentBattlePoints += ((Amour)card).getBattlePoints();
                } else if (card.GetType().IsSubclassOf(typeof(Ally))) {
                    permanentBattlePoints += ((Ally)card).getBattlePoints();
                    //currentBattlePoints += ((Ally)card).getBattlePoints();
                }
                //} else {
                //    currentBattlePoints += ((Weapon)card).getBattlePoints();
                //}
                participationList.Add(card);
                tempList.Remove(card);
                Debug.Log(strategyOwner.getName() + "'s battle points for stage " + i + ": " + currentBattlePoints);
                if (currentBattlePoints >= previousBattlePoints + 10) {
                    break;
                }
            }
            sortedList = new List<Card>(tempList);

            if (currentBattlePoints < previousBattlePoints + 10) {
                Debug.Log("Insufficient incrementable cards for " + strategyOwner.getName());
                return false;
            }
        }
        Debug.Log("Sufficient incrementable cards for " + strategyOwner.getName());
        return true;
    }

    public override List<Card> ParticipateTournament() {
        Logger.getInstance().info(strategyOwner.getName() + " is preparing for tournament");
        Debug.Log("AI is preparing for tournament");
        List<Card> Hand = strategyOwner.getHand();
        List<Card> PlayedList = new List<Card>();
        List<Card> Sorted = new List<Card>();
        Sorted = SortBattlePointsCards(Hand);
        int totalBattlePoints = 0;

        while(Sorted.Count > 0) {
            Card tempCard = GetHighestCard(Sorted);
            if (tempCard == null)
            {
                break;
            }
            else if (totalBattlePoints >= 50) {
                break;
            }
            if (!PlayedList.Contains(tempCard))
            {
                Debug.Log("Adding " + tempCard.getCardName() + " to AI");
                PlayedList.Add(tempCard);
                totalBattlePoints += ((Adventure)tempCard).getBattlePoints();
                //if(tempCard.GetType() == typeof(Amour) ){
                //    TotalBattlePoints += ((Amour)tempCard).getBattlePoints();
                //}
                //else if (tempCard.GetType().IsSubclassOf(typeof(Ally)))
                //{
                //    TotalBattlePoints += ((Ally)tempCard).getBattlePoints();
                //}
                //else {
                //    TotalBattlePoints += ((Weapon)tempCard).getBattlePoints();
                //}
            }
            Sorted.Remove(tempCard);
        }

        foreach(Card card in PlayedList) {
            Debug.Log("Cards AI will play is: " + card.getCardName());
        }
        return PlayedList;
    }

    Card GetHighestCard(List<Card> Sorted) {
        int HighestBattlePoint = 0;
        Card currentCard = null;

        foreach(Card card in Sorted) {
            if (currentCard == null || HighestBattlePoint < ((Adventure)card).getBattlePoints()) {
                currentCard = card;
                HighestBattlePoint = ((Adventure)card).getBattlePoints();
            }
            //if(card.GetType() == typeof(Amour)) {
            //    if(currentCard == null || HighestBattlePoint < ((Amour)card).getBattlePoints() ){
            //        currentCard = card;
            //        HighestBattlePoint = ((Amour)card).getBattlePoints();
            //    }
            //}
            //else if (card.GetType().IsSubclassOf(typeof(Ally))) {
            //    if (currentCard == null || HighestBattlePoint < ((Ally)card).getBattlePoints())
            //    {
            //        currentCard = card;
            //        HighestBattlePoint = ((Ally)card).getBattlePoints();
            //    }
            //}
            //else {
            //    if (currentCard == null || HighestBattlePoint < ((Weapon)card).getBattlePoints())
            //    {
            //        currentCard = card;
            //        HighestBattlePoint = ((Weapon)card).getBattlePoints();
            //    }
            //}
        }
        return currentCard;
    }


}
