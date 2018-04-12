using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Strategy2 : AbstractAI
{
    int previousStageBattlePoints = 0;
    int round = 1;

    public Strategy2() : base (40, 25) {
        
    }

    public override void DiscardAfterWinningTest(int currentBid, Quest quest)
    {
        Logger.getInstance().info("Discarding cards from AI after winning test");
        if(round == 1) { // If you get here, the currentBid will equal to number of foes they have in their hand, so just remove
            Logger.getInstance().info("if the stage number is 0, only remove the foe cards.");
            RemoveFoeCards(2);
        }
        else {
            Logger.getInstance().info("if the stage number is not 0, remove the foe and duplicates.");
            RemoveFoeAndDuplicateCards(2);
        }
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

    public override void NextBid(int currentBid, Stage stage)
    {
        Logger.getInstance().info(strategyOwner.getName() + " participating in Test and preparing");
        Debug.Log(strategyOwner.getName() + " participating in Test and preparing");
        if(round == 1){
            Logger.getInstance().info("First stage, only discarding foes in hand less than 25");
            Logger.getInstance().info("Foe bid is on first stage: " + GetTotalAvailableFoeBids(2));
            if (GetTotalAvailableFoeBids(2) > currentBid && GetTotalAvailableFoeBids(2) < discardableCardsThreshold)
            {
                foreach(Card card in strategyOwner.GetHand()){
                    Debug.Log("CARDS ARE: " + card.GetCardName());
                }
                Logger.getInstance().info(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(2));
                Debug.Log(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(2));
                stage.PromptTestResponse(false, GetTotalAvailableFoeBids(2));
                round++;
            }
            else
            {
                Logger.getInstance().info(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids(2)
                                  + " while currentbid is: " + currentBid + " AI dropping out.");
                Debug.Log(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids(2)
                                          + " while currentbid is: " + currentBid + " AI dropping out.");
                DropoutTest(currentBid, stage);
            }
        }
        else if (round == 2) {
            Logger.getInstance().info("Second stage, discarding foes and duplicates");
            Logger.getInstance().info("Inside second stage for AI");
            Debug.Log("Inside second stage for AI");
            Logger.getInstance().info("Foe bid is on not the first stage: " + GetTotalAvailableFoeBids(2));
            Logger.getInstance().info("Foe and Dup bid is: " + getTotalAvailableFoeandDuplicateBids(2));
            if (getTotalAvailableFoeandDuplicateBids(2) > currentBid && GetTotalAvailableFoeBids(2) < discardableCardsThreshold)
            {
                Logger.getInstance().info(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(2));
                Debug.Log(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(2));
                stage.PromptTestResponse(false, getTotalAvailableFoeandDuplicateBids(2));
            }
            else
            {
                Logger.getInstance().info(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids(2)
                                  + " while currentbid is: " + currentBid + " AI dropping out.");
                Debug.Log(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids(2)
                                          + " while currentbid is: " + currentBid + " AI dropping out.");
                DropoutTest(currentBid, stage);
            }
        }
        else {
            Logger.getInstance().info(strategyOwner.getName() + " AI Strategy 2 past round 2, won't bid: " + GetTotalAvailableFoeBids(2)
                           + " while currentbid is: " + currentBid + " AI dropping out.");
            Debug.Log(strategyOwner.getName() + " AI Strategy 2 past round 2, won't to bid: " + GetTotalAvailableFoeBids(2)
                                      + " while currentbid is: " + currentBid + " AI dropping out.");
            DropoutTest(currentBid, stage);
        }

    }

    public override void SponsorQuest()
    {
        Logger.getInstance().info(strategyOwner.getName() + " is preparing the quest");
        Debug.Log(strategyOwner.getName() + " is preparing the quest.");
        List<Stage> stages = new List<Stage>();
        Quest quest = (Quest)board.getCardInPlay();
        List<Adventure> cards = strategyOwner.GetHand();

        Stage finalStage = null;
        Stage testStage = null;
        List<Stage> otherStages = new List<Stage>();
        int initializedStages = 0, numTestStages = 0;

        Adventure stageCard = null;
        List<Adventure> weapons = new List<Adventure>();
        foreach (Adventure card in cards) {
            if (card.IsFoe()) {
                if (stageCard == null || card.getBattlePoints() > stageCard.getBattlePoints()) {
                    stageCard = card;
                }
            }
        }
        Logger.getInstance().info("Final stage foe: " + stageCard.GetCardName());
        Debug.Log("Final stage foe: " + stageCard.GetCardName());
        while (stageCard.getBattlePoints() + GetTotalBattlePoints(weapons) < minimumFinalStageBattlePoints) {
            weapons.Add(GetBestUniqueWeapon(cards, weapons));
        }

        Logger.getInstance().info("Final stage weapons: " + stageCard.GetCardName());
        Debug.Log("Final stage weapons:");
        foreach (Weapon weapon in weapons) {
            Logger.getInstance().info(weapon.GetCardName());
            Debug.Log(weapon.GetCardName());
        }
        finalStage = InitializeStage(stageCard, weapons, quest.getNumStages() - 1);
        initializedStages++;
        Logger.getInstance().info("Initialized stages " + initializedStages);
        Debug.Log("Initialized stages: " + initializedStages);

        if (ContainsTest(cards)) {
            Debug.Log(strategyOwner.getName() + " has a test in their hand.");
            Logger.getInstance().info(strategyOwner.getName() + " has a test in their hand.");
            foreach (Adventure card in cards) {
				if (card.IsTest()) {
                    stageCard = card;
                    break;
                }
            }
            testStage = InitializeStage(stageCard, null, quest.getNumStages() - 2);
            initializedStages++;
            numTestStages++;
            Debug.Log("Initialized stages: " + initializedStages);
            Logger.getInstance().info("Initialized stages: " + initializedStages);
        }

        Adventure previousStageCard = null;
        while (initializedStages < quest.getNumStages()) {
            stageCard = GetWeakestFoe(cards, previousStageCard);
            int stageNum = initializedStages - (numTestStages + 1);
            Logger.getInstance().info("Stage " + stageNum + ": stage card is " + stageCard.GetCardName());
            Debug.Log("Stage " + stageNum + ": stage card is " + stageCard.GetCardName());
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

    public override void PlayFoeStage(Stage stage)
    {
        Debug.Log("Stage card type is Foe");
        Logger.getInstance().info("Stage card type is Foe");
        Quest quest = (Quest)board.getCardInPlay();
        List<Adventure> cards = strategyOwner.GetHand();
        List<Adventure> sortedList = SortCardsByType(cards);
        List<Adventure> participationList = new List<Adventure>();

        if (stage.getStageNum() == quest.getNumStages() - 1) {
            foreach (Adventure card in sortedList) {
                Debug.Log("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.GetCardName());
                Logger.getInstance().info("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.GetCardName());
                if (CanPlayCardForStage(card, participationList)) {
                    Debug.Log(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    Logger.getInstance().info(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    participationList.Add(card);
                }
            }
        } else {
            Debug.Log("Not the final stage. Play in increments of 10");
            Logger.getInstance().info("Not the final stage. Play in increments of 10");
            int currentBattlePoints = strategyOwner.getPlayArea().GetBattlePoints();
            Debug.Log("Minimum battle points to pass: " + (previousStageBattlePoints + 10));
            Logger.getInstance().info("Minimum battle points to pass: " + (previousStageBattlePoints + 10));
            foreach (Adventure card in sortedList) {
                Debug.Log("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.GetCardName());
                Logger.getInstance().info("Checking " + strategyOwner.getName() + "'s card for eligibility: " + card.GetCardName());
                if (CanPlayCardForStage(card, participationList)) {
                    Debug.Log(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    Logger.getInstance().info(strategyOwner.getName() + " can play card, adding to participation list for stage");
                    participationList.Add(card);
                    currentBattlePoints += card.getBattlePoints();
                    Debug.Log(strategyOwner.getName() + "'s current battle points: " + currentBattlePoints);
                    Logger.getInstance().info(strategyOwner.getName() + "'s current battle points: " + currentBattlePoints);
                    if (currentBattlePoints >= previousStageBattlePoints + 10) {
                        Debug.Log("Sufficient battle points acquired, moving on with stage");
                        Logger.getInstance().info("Sufficient battle points acquired, moving on with stage");
                        break;
                    }
                }
            }
            if (currentBattlePoints < previousStageBattlePoints + 10) {
                Debug.Log("Whoopsies, somehow the participation condition was violated. Dropping out of quest.");
                Logger.getInstance().info("Whoopsies, somehow the participation condition was violated. Dropping out of quest.");
                stage.PromptFoeResponse(true);
            }
            previousStageBattlePoints = currentBattlePoints;
        }
        foreach (Adventure card in participationList) {
            Logger.getInstance().info("Moving card from " + strategyOwner.getName() + "'s hand to play area: " + card.GetCardName());
            strategyOwner.getPlayArea().AddCard(card);
            strategyOwner.RemoveCard(card);
        }
        stage.PromptFoeResponse(false);
    }

    bool IncrementableCardsOverEachStage() {
        Quest quest = (Quest)board.getCardInPlay();
        List<Adventure> cards = strategyOwner.GetHand();
        List<Adventure> sortedList = SortCardsByType(cards);
        List<Adventure> participationList = new List<Adventure>();

        int previousBattlePoints = 0;
        int permanentBattlePoints = 0;
        int currentBattlePoints = 0;
        for (int i = 0; i < quest.getNumStages(); i++) {
            Debug.Log("Calculating " + strategyOwner.getName() + "'s validity for stage " + i);
            Logger.getInstance().info("Calculating " + strategyOwner.getName() + "'s validity for stage " + i);
            List<Adventure> tempList = new List<Adventure>(sortedList);
            previousBattlePoints = currentBattlePoints;
            currentBattlePoints = permanentBattlePoints;
            Debug.Log("Required battle points: " + (previousBattlePoints + 10));
            Logger.getInstance().info("Required battle points: " + (previousBattlePoints + 10));

            foreach (Adventure card in sortedList) {
                Debug.Log("Adding " + card.GetCardName() + " to " + strategyOwner.getName() + "'s hypothetical play area");
                Logger.getInstance().info("Adding " + card.GetCardName() + " to " + strategyOwner.getName() + "'s hypothetical play area");
                currentBattlePoints += card.getBattlePoints();
				if (card.IsAmour()) {
                    permanentBattlePoints += card.getBattlePoints();
                } else if (card.IsAlly()) {
                    permanentBattlePoints += card.getBattlePoints();
                }
                participationList.Add(card);
                tempList.Remove(card);
                Debug.Log(strategyOwner.getName() + "'s battle points for stage " + i + ": " + currentBattlePoints);
                Logger.getInstance().info(strategyOwner.getName() + "'s battle points for stage " + i + ": " + currentBattlePoints);
                if (currentBattlePoints >= previousBattlePoints + 10) {
                    break;
                }
            }
            sortedList = new List<Adventure>(tempList);

            if (currentBattlePoints < previousBattlePoints + 10) {
                Debug.Log("Insufficient incrementable cards for " + strategyOwner.getName());
                Logger.getInstance().info("Insufficient incrementable cards for " + strategyOwner.getName());
                return false;
            }
        }
        Debug.Log("Sufficient incrementable cards for " + strategyOwner.getName());
        Logger.getInstance().info("Sufficient incrementable cards for " + strategyOwner.getName());
        return true;
    }

    public override List<Adventure> ParticipateTournament() {
        Logger.getInstance().info(strategyOwner.getName() + " is preparing for tournament");
        Debug.Log("AI is preparing for tournament");
        List<Adventure> Hand = strategyOwner.GetHand();
        List<Adventure> PlayedList = new List<Adventure>();
        List<String> PlayedListName = new List<string>();
        List<Adventure> Sorted = new List<Adventure>();
        Sorted = SortCardsByType(Hand);
        int totalBattlePoints = 0;

        while(Sorted.Count > 0) {
            Adventure tempCard = GetHighestCard(Sorted);
            if (tempCard == null)
            {
                break;
            }
            else if (totalBattlePoints >= 50) {
                break;
            }
            if (!PlayedListName.Contains(tempCard.GetCardName()))
            {
                Debug.Log("Adding " + tempCard.GetCardName() + " to AI");
                Logger.getInstance().info("Adding " + tempCard.GetCardName() + " to AI");
                PlayedList.Add(tempCard);
                PlayedListName.Add(tempCard.GetCardName());
                totalBattlePoints += tempCard.getBattlePoints();
            }
            Sorted.Remove(tempCard);
        }

        foreach(Adventure card in PlayedList) {
            Debug.Log("Cards AI will play is: " + card.GetCardName());
            Logger.getInstance().info("Cards AI will play is: " + card.GetCardName());
        }
        return PlayedList;
    }

    Adventure GetHighestCard(List<Adventure> Sorted) {
        int HighestBattlePoint = 0;
        Adventure currentCard = null;

        foreach(Adventure card in Sorted) {
			if (currentCard == null || HighestBattlePoint < card.getBattlePoints ()) {
				currentCard = card;
				HighestBattlePoint = card.getBattlePoints ();
			}
        }
        return currentCard;
    }


}
