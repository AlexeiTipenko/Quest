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
        //if(TwoWeaponsorAlliesPerStage()){
        //    return true;
        //}
        //else {
        //    return false;
        //}
        return true;
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
        Logger.getInstance().info(strategyOwner.getName() + " AI Strat 1 is peparing bids");
        Debug.Log("AI Strat 1 is preparing bids");

        if (GetTotalAvailableFoeBids() > currentBid && GetTotalAvailableFoeBids() < discardableCardsThreshold)
        {
            Logger.getInstance().info(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids());
            Debug.Log(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids());
            stage.PromptTestResponse(false, GetTotalAvailableFoeBids());
        }
        else
        {
            Logger.getInstance().info(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids()
                                      + " while currentbid is: " + currentBid + " AI dropping out.");
            Debug.Log(strategyOwner.getName() + " AI doesn't have enough to bid: " + GetTotalAvailableFoeBids()
                                      + " while currentbid is: " + currentBid + " AI dropping out.");
            stage.PromptTestResponse(true, 0);
        }
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
		Dictionary<Card, bool> cardDictionary = new Dictionary<Card, bool> ();
		foreach (Card card in cards) {
			cardDictionary.Add (card, false);
		}
		Debug.Log ("Card dictionary contains:");
		foreach (Card card in cardDictionary.Keys) {
			Debug.Log ("Card: " + card);
		}

        Stage finalStage = null;
        Stage testStage = null;
        List<Stage> otherStages = new List<Stage>();
        int initializedStages = 0, numTestStages = 0;

		int previousStageBattlePoints = 0;

        Card stageCard = null;
        List<Card> weapons = new List<Card>();
		stageCard = GetStrongestFoe (cardDictionary);
		if (stageCard == null) {
			Debug.Log ("Failed to initialize all stages, withdrawing sponsorship");
			quest.IncrementSponsor ();
			return;
		}
		cardDictionary [stageCard] = true;
        Logger.getInstance().info("Final stage foe: " + stageCard.getCardName());
        Debug.Log("Final stage foe: " + stageCard.getCardName());
        while (((Foe)stageCard).getBattlePoints() + GetTotalBattlePoints(weapons) < minimumFinalStageBattlePoints)
        {
			Weapon bestUniqueWeapon = GetBestUniqueWeapon (new List<Card>(cardDictionary.Keys), weapons);
			if (bestUniqueWeapon == null) {
				Debug.Log ("No more weapons found");
				break;
			}
			cardDictionary [bestUniqueWeapon] = true;
            weapons.Add(bestUniqueWeapon);
        }

		Logger.getInstance().info("Final stage weapons:");
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
		previousStageBattlePoints = finalStage.getTotalBattlePoints ();

		if (ContainsTest(new List<Card>(cardDictionary.Keys)))
        {
            Debug.Log(strategyOwner.getName() + " has a test in their hand.");
			foreach (Card card in cardDictionary.Keys)
            {
                if (card.GetType().IsSubclassOf(typeof(Test)))
                {
                    stageCard = card;
                    break;
                }
            }
			cardDictionary [stageCard] = true;
            testStage = InitializeStage(stageCard, null, quest.getNumStages() - 2);
            initializedStages++;
            numTestStages++;
            Debug.Log("Initialized stages: " + initializedStages);
        }

        while (initializedStages < quest.getNumStages())
        {
			Debug.Log ("Initializing a new stage");
			stageCard = GetStrongestFoe (cardDictionary);
			if (stageCard == null) {
				break;
			}
			List<Card> weaponList = null;
			Weapon weaponCard = GetBestDuplicateWeapon (cardDictionary);
			cardDictionary [stageCard] = true;
			if (weaponCard != null) {
				cardDictionary [weaponCard] = true;
				weaponList = new List<Card> ();
				weaponList.Add (weaponCard);
			}
			if (IsValidBattlePoints ((Foe)stageCard, weaponCard, previousStageBattlePoints)) {
				Debug.Log ("Created a valid quest");
				int stageNum = quest.getNumStages () - initializedStages - 1;
				Logger.getInstance().info("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
				Debug.Log("Stage " + stageNum + ": stage card is " + stageCard.getCardName());
				Stage newStage = InitializeStage (stageCard, weaponList, stageNum);
				otherStages.Insert(0, newStage);
				initializedStages++;
				previousStageBattlePoints = newStage.getTotalBattlePoints ();
			} else {
				Debug.Log ("Invalid battle point combo");
				if (weaponCard != null) {
					Debug.Log ("Removing weapon");
					cardDictionary.Remove (weaponCard);
				} else {
					Debug.Log ("Removing stage card");
					cardDictionary.Remove (stageCard);
				}
			}
        }
		if (initializedStages == quest.getNumStages ()) {
			Debug.Log ("All stages have been initialized");
			foreach (Stage stage in otherStages) {
				stages.Add (stage);
			}
			if (testStage != null)
			{
				stages.Add(testStage);
			}
			stages.Add(finalStage);
			quest.SponsorQuestComplete(stages);
		} else {
			Debug.Log ("Failed to initialize all stages, withdrawing sponsorship");
			quest.IncrementSponsor ();
		}
    }

	private bool IsValidBattlePoints(Foe stageCard, Weapon weaponCard, int previousStageBattlePoints) {
		int stageCardBattlePoints = 0, weaponCardBattlePoints = 0;
		stageCardBattlePoints = stageCard.getBattlePoints ();
		if (weaponCard != null) {
			weaponCardBattlePoints = weaponCard.getBattlePoints ();
		}
		if (stageCardBattlePoints + weaponCardBattlePoints >= previousStageBattlePoints) {
			return false;
		}
		return true;
	}

    protected override bool CanPlayCardForStage(Card card, List<Card> participationList)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayFoeStage(Stage stage)
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
