using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


[Serializable]
public class Strategy1 : AbstractAI
{
    int round = 1;

    public Strategy1() : base (50, 20) {
        
    }

    public override void DiscardAfterWinningTest(int currentBid, Quest quest)
    {
        RemoveFoeCards(1);
    }

    public override bool DoIParticipateInQuest()
    {
        Logger.getInstance().info("AI Strategy 1 deciding if participating in quest");
        Debug.Log("AI Strategy 1 deciding if participating in quest");
        if(TwoWeaponsorAlliesPerStage() && FoesUnder20()){
            Logger.getInstance().info("AI Strategy1" + strategyOwner.getName() + " playing in quest");
            Debug.Log("AI Strategy1" + strategyOwner.getName() + " playing in quest");
            foreach(Card card in strategyOwner.getHand()){
                Debug.Log(strategyOwner.getName() + "Cards are: " + card.GetCardName());
            }
            return true;
        }
        else {
            Logger.getInstance().info("AI Strategy1" + strategyOwner.getName() + " not playing in quest");
            Debug.Log("AI Strategy1" + strategyOwner.getName() + " not playing in quest");
            foreach (Card card in strategyOwner.getHand())
            {
                Debug.Log(strategyOwner.getName() + " not participating in quest Cards are: " + card.GetCardName());
            }
            return false;
        }
    }

    public override bool DoIParticipateInTournament()
    {
		return PlayersCanEvolveOrWinWithTournament (board.getPlayers ());
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
        if(round == 1) {
            if (GetTotalAvailableFoeBids(1) > currentBid && GetTotalAvailableFoeBids(1) < discardableCardsThreshold)
            {
                Logger.getInstance().info(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(1));
                Debug.Log(strategyOwner.getName() + " AI is preparing to bid: " + GetTotalAvailableFoeBids(1));
                stage.PromptTestResponse(false, GetTotalAvailableFoeBids(1));
            }
            else {
                DropoutTest(currentBid, stage);
            }
            round++;
        }
        else
        {
            Logger.getInstance().info(strategyOwner.getName() + " AI Strat 1 doesn't have enough to bid: " + GetTotalAvailableFoeBids(1)
                                      + " while currentbid is: " + currentBid + " AI dropping out.");
            Debug.Log(strategyOwner.getName() + " AI Strat 1 doesn't have enough to bid: " + GetTotalAvailableFoeBids(1)
                                      + " while currentbid is: " + currentBid + " AI dropping out.");
            DropoutTest(currentBid, stage);
        }
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
        Logger.getInstance().info("Final stage foe: " + stageCard.GetCardName());
        Debug.Log("Final stage foe: " + stageCard.GetCardName());
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
            Logger.getInstance().info(weapon.GetCardName());
            Debug.Log(weapon.GetCardName());
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
				if (card.IsTest())
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
				Logger.getInstance().info("Stage " + stageNum + ": stage card is " + stageCard.GetCardName());
				Debug.Log("Stage " + stageNum + ": stage card is " + stageCard.GetCardName());
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

    public override void PlayFoeStage(Stage stage)
    {
		Debug.Log ("Playing out foe for AI strategy 2");
		List<Card> hand = strategyOwner.getHand ();
		List<Adventure> sortedList = SortCardsByBattlePoints (hand);
		List<Adventure> participationList = new List<Adventure> ();
		Quest quest = (Quest)board.getCardInPlay ();

		if (stage.getStageNum () == quest.getNumStages () - 1) {
			Debug.Log ("Final stage! UNLIMITED POWAAAAAAAAAAAAAAAAR");
			foreach (Adventure card in sortedList) {
				Debug.Log ("Checking " + strategyOwner.getName () + "'s card for eligibility: " + card.GetCardName ());
				if (CanPlayCardForStage (card, participationList)) {
					Debug.Log (strategyOwner.getName () + " can play card, adding to participation list for stage");
					participationList.Add (card);
				}
			}
		} else {
			int numAvailableAllyAndAmourCards = 0;
			foreach (Adventure card in sortedList) {
				if (card.IsAlly() || card.GetType () == typeof(Amour)) {
					numAvailableAllyAndAmourCards++;
				}
			}
			if (numAvailableAllyAndAmourCards > 0) {
				foreach (Adventure card in sortedList) {
					if (card.IsAlly() || card.GetType () == typeof(Amour)) {
						if (CanPlayCardForStage (card, participationList)) {
							Debug.Log (strategyOwner.getName () + " can play card, adding to participation list for stage");
							participationList.Add (card);
							if (participationList.Count > 1) {
								break;
							}
						}
					}
				}
			}
			while (participationList.Count < 2) {
				Adventure weaponToPlay = null;
				for (int i = sortedList.Count - 1; i >= 0; i--) {
					Adventure card = sortedList.ElementAt (i);
					if (card.IsWeapon()) {
						if (weaponToPlay == null || card.getBattlePoints () < weaponToPlay.getBattlePoints ()) {
							if (CanPlayCardForStage(card, participationList)) {
								weaponToPlay = card;
							}
						}
					}
				}
				if (weaponToPlay == null) {
					break;
				}
				participationList.Add (weaponToPlay);
			}
		}
		foreach (Adventure card in participationList) {
			Debug.Log("Moving card from " + strategyOwner.getName() + "'s hand to play area: " + card.GetCardName());
			strategyOwner.getPlayArea().addCard(card);
			strategyOwner.RemoveCard(card);
		}
		stage.PromptFoeResponse(false);
    }

    public override List<Card> ParticipateTournament() {
		Tournament tournament = (Tournament)board.getCardInPlay();
		List<Card> hand = strategyOwner.getHand();
		List<Adventure> sortedList = SortCardsByType (hand);
		List<Card> participationList = new List<Card> ();
		if (PlayersCanEvolveOrWinWithTournament (tournament.participatingPlayers)) {
			//play strongest possible hand (includes allies, amours and weapons)
			foreach (Card card in sortedList) {
				if (((card.GetType () == typeof(Amour) || card.IsWeapon()) && !participationList.Contains (card)) 
					|| card.IsAlly())
				{
					participationList.Add (card);
				} 
			}
		} else {
			//Else: I play only weapons I have two or more instances of
			List<Card> weaponList = new List<Card>();
			foreach (Card card in sortedList) {
				if (card.IsWeapon()) {
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

    public bool TwoWeaponsorAlliesPerStage()
    {
        Quest quest = (Quest)board.getCardInPlay();
        int numWeaponsAndAllies = 0;
        foreach (Card card in strategyOwner.getHand())
        {
            if (card.IsWeapon() || card.IsAlly() || card.IsAmour())
            {
                numWeaponsAndAllies++;
            }
        }
        if (numWeaponsAndAllies >= (quest.getNumStages() * 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FoesUnder20()
    {
        int foes = 0;
        foreach (Card card in strategyOwner.getHand())
        {
            if (card.IsFoe())
            {
                Foe tempcard = (Foe)card;
                if (tempcard.getBattlePoints() < 20)
                {
                    foes++;
                }
            }
        }
        if (foes >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	private bool PlayersCanEvolveOrWinWithTournament(List<Player> players) {
		Debug.Log("Checking if someone else can win or evolve through this tournament.");
		Logger.getInstance().info("AI Strategy 1 checking if someone else can win/evolve through tournament");
		Tournament tournament = (Tournament)board.getCardInPlay();
		foreach (Player player in players) {
			if (player.getNumShields() + tournament.GetBonusShields() >= player.getRank().getShieldsToProgress()) {
				Debug.Log("Player " + player.getName() + " can win off this tournament.");
				Logger.getInstance().info("AI Strategy 1 found that Player: " + player.getName() + " can win off this tournament.");
				return true;
			}
		}
		Debug.Log("No player can win off this tournament.");
		return false;
	}
}
