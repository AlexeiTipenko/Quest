using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public abstract class AbstractAI {
    
    protected readonly BoardManagerMediator board;
    protected AIPlayer strategyOwner;
    protected int minimumFinalStageBattlePoints, discardableCardsThreshold;

    protected AbstractAI(int minimumFinalStageBattlePoints, int discardableCardsThreshold) {
        board = BoardManagerMediator.getInstance();
        this.minimumFinalStageBattlePoints = minimumFinalStageBattlePoints;
        this.discardableCardsThreshold = discardableCardsThreshold;
    }
    
    public abstract bool DoIParticipateInTournament();

    public abstract bool DoISponsorAQuest();

    public abstract bool DoIParticipateInQuest();

    public abstract void NextBid(int currentBid, Stage stage);

    public abstract void DiscardAfterWinningTest(int currentBid, Quest quest);

    public abstract void SponsorQuest();

    public abstract void PlayFoeStage(Stage stage);

    public abstract List<Adventure> ParticipateTournament();

    //--------------------------------------------//
    //---------- Non-Abstract Functions ----------//
    //--------------------------------------------//

    public void SetStrategyOwner(AIPlayer strategyOwner) {
        this.strategyOwner = strategyOwner;
    }

    protected bool SomeoneElseCanWinOrEvolveWithQuest() {
        Debug.Log("Checking if someone else can win or evolve through this quest.");
		Logger.getInstance().info("Checking if someone else can win or evolve through this quest.");
        Quest quest = (Quest)board.getCardInPlay();
        foreach (Player player in board.getPlayers()) {
            if (player != strategyOwner) {
                if (player.getNumShields() + quest.getNumStages() >= player.getRank().getShieldsToProgress()) {
                    Debug.Log("Player " + player.getName() + " can win off this quest.");
					Logger.getInstance().info("Player " + player.getName() + " can win off this quest.");
                    return true;
                }
            }
        }
        Debug.Log("No player can win off this quest.");
		Logger.getInstance().info("No player can win off this quest.");
        return false;
    }

    protected bool SufficientCardsToSponsorQuest() {
        Debug.Log("Checking if " + strategyOwner.getName() + " has sufficiently valid cards to sponsor quest.");
		Logger.getInstance().info("Checking if " + strategyOwner.getName() + " has sufficiently valid cards to sponsor quest.");
        Quest quest = (Quest)board.getCardInPlay();
        List<Adventure> cards = strategyOwner.GetHand();
        List<Adventure> validCards = new List<Adventure>();
        HashSet<int> uniqueBattlePoints = new HashSet<int>();
        HashSet<Adventure> uniqueWeapons = new HashSet<Adventure>();
        int validCardCount = 0;
        foreach (Adventure card in cards) {
			if (card.IsFoe()) {
                validCards.Add(card);
                validCardCount++;
                uniqueBattlePoints.Add(card.getBattlePoints());
            }
			else if (card.IsTest()) {
                if (!ContainsTest(validCards)) {
                    validCardCount++;
                }
                validCards.Add(card);
			} else if (card.IsWeapon()) {
				bool unique = true;
				foreach (Adventure weapon in uniqueWeapons) {
					if (weapon.GetCardName () == card.GetCardName ()) {
						unique = false;
						break;
					}
				}
				if (unique) {
					uniqueWeapons.Add(card);
				}
            }
        }
		foreach (Adventure card in uniqueWeapons) {
			Debug.Log("Unique weapons: " + card.GetCardName());
			Logger.getInstance().info("Unique weapons: " + card.GetCardName());
		}
        if (validCardCount >= quest.getNumStages()) {
            Debug.Log(strategyOwner.getName() + " has enough valid cards.");
			Logger.getInstance().info(strategyOwner.getName() + " has enough valid cards.");
            int totalWeaponsBattlePoints = 0;
            foreach (Adventure weapon in uniqueWeapons) {
                totalWeaponsBattlePoints += weapon.getBattlePoints();
            }
            if (totalWeaponsBattlePoints + HighestBattlePoints(uniqueBattlePoints) >= minimumFinalStageBattlePoints) {
                Debug.Log(strategyOwner.getName() + " has sufficient cards to meet minimumFinalStageBattlePoints requirement.");
				Logger.getInstance().info(strategyOwner.getName() + " has sufficient cards to meet minimumFinalStageBattlePoints requirement.");
                if (!ContainsTest(validCards)) {
					if (uniqueBattlePoints.Count >= quest.getNumStages ()) {
						Debug.Log(strategyOwner.getName() + " has sufficient cards (without using a test) to sponsor this quest.");
						Logger.getInstance().info(strategyOwner.getName() + " has sufficient cards (without using a test) to sponsor this quest.");
						return true;
					}
                } else {
					if (uniqueBattlePoints.Count >= (quest.getNumStages () - 1)) {
						Debug.Log(strategyOwner.getName() + " has sufficient cards (using a test) to sponsor this quest.");
						Logger.getInstance().info(strategyOwner.getName() + " has sufficient cards (using a test) to sponsor this quest.");
						return true;
					}
                }
            }
        }
		Debug.Log ("Didn't meet battlepoint requirements");
		Logger.getInstance().info ("Didn't meet battlepoint requirements");
        return false;
    }

    protected bool ContainsTest(List<Adventure> cards) {
        foreach (Adventure card in cards) {
			if (card.IsTest()) {
                return true;
            }
        }
        return false;
    }

    int HighestBattlePoints(HashSet<int> uniqueBattlePoints) {
        int highestBattlePoints = 0;
        foreach (int battlePoints in uniqueBattlePoints) {
            if (battlePoints > highestBattlePoints) {
                highestBattlePoints = battlePoints;
            }
        }
        return highestBattlePoints;
    }

    protected int GetTotalBattlePoints(List<Adventure> cards) {
        int totalBattlePoints = 0;
        foreach (Adventure card in cards) {
            totalBattlePoints += card.getBattlePoints();
        }
        return totalBattlePoints;
    }

	protected List<Adventure> SortCardsByType(List<Adventure> cards) {
		Adventure amour = null;
		List<Adventure> allies = new List<Adventure>();
		List<Adventure> weapons = new List<Adventure>();
		List<Adventure> sortedList = new List<Adventure>();
		Debug.Log("Available cards:");
		Logger.getInstance().info("Available cards:");
		foreach (Adventure card in cards) {
			Debug.Log(card.GetCardName());
			Logger.getInstance().info(card.GetCardName());
		}
		Debug.Log("Looping through cards");
		Logger.getInstance().info("Looping through cards");
		foreach (Adventure card in cards)
		{
			bool inserted = false;
			if (card.IsAmour())
			{
				amour = card;
			}
			else if (card.IsAlly())
			{
				List<Adventure> tempAllies = new List<Adventure>(allies);
				foreach (Ally ally in allies)
				{
					if (card.getBattlePoints() <= ally.getBattlePoints())
					{
						tempAllies.Insert(tempAllies.IndexOf(ally), card);
						inserted = true;
						break;
					}
				}
				if (!inserted) {
					tempAllies.Add(card);
				}
				allies = new List<Adventure>(tempAllies);
			}
			else if (card.IsWeapon())
			{
				List<Adventure> tempWeapons = new List<Adventure>(weapons);
				foreach (Weapon weapon in weapons)
				{
					if (card.getBattlePoints() <= weapon.getBattlePoints())
					{
						tempWeapons.Insert(tempWeapons.IndexOf(weapon), card);
						inserted = true;
						break;
					}
				}
				if (!inserted) {
					tempWeapons.Add(card);
				}
				weapons = new List<Adventure>(tempWeapons);
			}
		}
		if (amour != null) {
			sortedList.Add(amour);
		}
		foreach (Ally ally in allies)
		{
			sortedList.Add(ally);
		}
		foreach (Weapon weapon in weapons)
		{
			sortedList.Add(weapon);
		}
		Debug.Log("Sorted valid cards in hand:");
		Logger.getInstance().info("Sorted valid cards in hand:");
		foreach (Adventure card in sortedList) {
			Debug.Log(card.GetCardName());
			Logger.getInstance().info(card.GetCardName());
		}
		return sortedList;
	}

	protected List<Adventure> SortCardsByBattlePoints(List<Adventure> cards) {
		List<Adventure> sortedCards = new List<Adventure> ();
		bool containsAmour = false;
		foreach (Card card in cards) {
			if (card.IsAlly() || card.IsWeapon() || card.IsAmour()) {
				int index = -1;
				foreach (Adventure sortedCard in sortedCards) {
					if (((Adventure)card).getBattlePoints () > sortedCard.getBattlePoints ()) {
						index = sortedCards.IndexOf (sortedCard);
					}
				}
				InsertIntoListAtIndex ((Adventure)card, sortedCards, index, containsAmour);
				if (card.IsAmour()) {
					containsAmour = true;
				}
			}
		}
		return sortedCards;
	}

	private List<Adventure> InsertIntoListAtIndex(Adventure card, List<Adventure> sortedCards, int index, bool containsAmour) {
		if (card.IsAmour() && containsAmour) {
			return sortedCards;
		}
		if (index == -1) {
			sortedCards.Add (card);
		} else {
			sortedCards.Insert (index, card);
		}
		return sortedCards;
	}

	protected bool CanPlayCardForStage(Adventure card, List<Adventure> participationList)
	{
		if (card.IsAmour()) {
			foreach (Card participationCard in participationList) {
				if (participationCard.IsAmour()) {
					return false;
				}
			}
			foreach (Card playAreaCard in strategyOwner.getPlayArea().getCards()) {
				if (playAreaCard.IsAmour()) {
					return false;
				}
			}
		}
		else if (card.IsWeapon()) {
			foreach (Card participationCard in participationList) {
				if (participationCard.GetType() == card.GetType()) {
					return false;
				}
			}
		}
		return true;
	}

    protected Adventure GetBestUniqueWeapon(List<Adventure> cards, List<Adventure> currentWeapons) {
		Debug.Log ("Getting best unique weapon");
		Logger.getInstance().info ("Getting best unique weapon");
        Adventure bestWeapon = null;
        foreach (Adventure card in cards) {
			if (card.IsWeapon()) {
				Debug.Log ("Found a weapon: " + card.GetCardName ());
				Logger.getInstance().info ("Found a weapon: " + card.GetCardName ());
				bool exists = false;
				foreach (Adventure weapon in currentWeapons) {
					if (weapon.GetCardName () == card.GetCardName ()) {
						Debug.Log ("Weapon has already been selected");
						Logger.getInstance().info ("Weapon has already been selected");
						exists = true;
						break;
					}
				}
                if (!exists) {
					Debug.Log ("Weapon has not been selected");
					Logger.getInstance().info ("Weapon has not been selected");
                    if (bestWeapon == null || card.getBattlePoints() > bestWeapon.getBattlePoints()) {
						Debug.Log ("Updating best weapon to: " + card.GetCardName ());
						Logger.getInstance().info ("Updating best weapon to: " + card.GetCardName ());
                        bestWeapon = card;
                    }
                }
            }
        }
        return bestWeapon;
    }

	protected Adventure GetBestDuplicateWeapon(Dictionary<Adventure, bool> cards) {
		Debug.Log ("Getting best duplicate weapon");
		Logger.getInstance().info ("Getting best duplicate weapon");
		Dictionary<string, List<Adventure>> cardLists = new Dictionary<string, List<Adventure>> ();
		foreach (Adventure card in cards.Keys) {
			if (card.IsWeapon() && !cards [card]) {
				Debug.Log ("Found an unused weapon: " + card.GetCardName ());
				Logger.getInstance().info ("Found an unused weapon: " + card.GetCardName ());
				string cardName = card.GetCardName ();
				List<Adventure> cardList = new List<Adventure> ();
				if (cardLists.ContainsKey (cardName)) {
					cardList = cardLists [cardName];
					cardLists.Remove (cardName);
				}
				cardList.Add (card);
				cardLists.Add (cardName, cardList);
			}
		}

		string bestWeaponName = null;
		foreach (string card in cardLists.Keys) {
			if (cardLists [card].Count > 1) {
				Debug.Log ("Duplicate weapon found: " + card);
				Logger.getInstance().info ("Duplicate weapon found: " + card);
				int newWeaponBattlePoints = cardLists[card][0].getBattlePoints ();
				if (bestWeaponName == null || newWeaponBattlePoints > cardLists[bestWeaponName][0].getBattlePoints ()) {
					Debug.Log ("Updated best duplicate weapon: " + card);
					Logger.getInstance().info ("Updated best duplicate weapon: " + card);
					bestWeaponName = card;
				}
			}
		}

		Adventure bestWeapon = null;
		if (bestWeaponName != null) {
			List<Adventure> cardList = cardLists [bestWeaponName];
			foreach (Adventure card in cardList) {
				if (!cards [card]) {
					Debug.Log ("Retrieved best duplicate weapon");
					Logger.getInstance().info ("Retrieved best duplicate weapon");
					bestWeapon = card;
					break;
				}
			}
		}
		return bestWeapon;
	}

    protected Stage InitializeStage(Adventure stageCard, List<Adventure> weapons, int stageNum) {
        Stage stage = new Stage(stageCard, weapons, stageNum);
        strategyOwner.RemoveCard(stageCard);
        if (weapons != null) {
            foreach (Adventure weapon in weapons) {
                strategyOwner.RemoveCard(weapon);
            }
        }
        return stage;
    }

    protected bool SufficientDiscardableCards() {
        List<Adventure> cards = strategyOwner.GetHand();
        int discardableCards = 0;
        foreach (Adventure card in cards) {
			if (card.IsFoe()) {
                if (card.getBattlePoints() < discardableCardsThreshold) {
                    discardableCards++;
                }
            }
        }
        Debug.Log(strategyOwner.getName() + " has " + discardableCards + " discardable cards.");
		Logger.getInstance().info(strategyOwner.getName() + " has " + discardableCards + " discardable cards.");
        return (discardableCards > 1);
    }

    public void DropoutTest(int currentBid, Stage stage) {
        stage.PromptTestResponse(true, 0);
    }

    public int GetTotalAvailableFoeBids(int strategy)
    {
        if(strategy == 2){
            int availableBids = 0;
            foreach (Adventure card in strategyOwner.GetHand())
            {
                if (card.IsFoe() && card.getBattlePoints() < discardableCardsThreshold)
                {
                    availableBids += 1;
                }
            }
            return availableBids;
        }
        else {
            int availableBids = 0;
            foreach (Adventure card in strategyOwner.GetHand())
            {
                if (card.IsFoe() && ((Foe)card).GetMinBattlePoints() < discardableCardsThreshold)
                {
                    availableBids += 1;
                }
            }
            return availableBids;
        }
    }

    public int getTotalAvailableFoeandDuplicateBids(int strategy)
    {
        int availableBids = 0;
        Dictionary<String, int> cardDictionary = new Dictionary<String, int>();
        foreach (Card card in strategyOwner.GetHand())
        {
			if (!cardDictionary.ContainsKey(card.GetCardName()) && !card.IsFoe())
            {
                Debug.Log("Inserting into Dictionary: " + card.GetCardName());
				Logger.getInstance().info("Inserting into Dictionary: " + card.GetCardName());
                cardDictionary.Add(card.GetCardName(), 1);
            }
			else if (!card.IsFoe())
            {
                cardDictionary[card.GetCardName()]++;
            }
        }
        foreach (KeyValuePair<String, int> entry in cardDictionary)
        {
            if (entry.Value > 1)
            {
                Debug.Log("Found Duplicate!: " + entry.Value + ", " + entry.Key);
				Logger.getInstance().info("Found Duplicate!: " + entry.Value + ", " + entry.Key);
                availableBids += entry.Value - 1;
            }
        }
        Debug.Log("Duplicates are: " + availableBids + " inside Foe and Dups");
		Logger.getInstance().info("Duplicates are: " + availableBids + " inside Foe and Dups");
        if (strategy == 1) {
            return GetTotalAvailableFoeBids(1) + availableBids;
        }
        else {
            return GetTotalAvailableFoeBids(2) + availableBids;
        }
    }

    public void RemoveFoeCards(int strategy)
    {
        if (strategy == 1){
            List<Adventure> TempHand = new List<Adventure>(strategyOwner.GetHand());
            foreach (Adventure card in TempHand)
            {
                if (card.IsFoe() && ((Foe)card).GetMinBattlePoints() < discardableCardsThreshold)
                {
                    strategyOwner.RemoveCard(card);
                }
            }  
        }
        else {
            List<Adventure> TempHand = new List<Adventure>(strategyOwner.GetHand());
            foreach (Adventure card in TempHand)
            {
                if (card.IsFoe() && card.getBattlePoints() < discardableCardsThreshold)
                {
                    strategyOwner.RemoveCard(card);
                }
            }   
        }
    }

    public void RemoveFoeAndDuplicateCards(int strategy)
    {
        List<String> Seen = new List<String>();
        List<Adventure> TempHand = new List<Adventure>(strategyOwner.GetHand());

        if (strategy == 1){
			RemoveFoeCards(1);
        }
        else {
            RemoveFoeCards(2);
        }

        foreach (Adventure card in TempHand)
        {
            if (!Seen.Contains(card.GetCardName()))
            {
                Seen.Add(card.GetCardName());
            }
            else
            {
                strategyOwner.RemoveCard(card);
            }
        }
    }

	public Adventure GetWeakestFoe(List<Adventure> cards, Adventure previousStageCard)
	{
		Adventure weakestFoe = null;
		foreach (Adventure card in cards)
		{
			if (card.IsFoe())
			{
				if (weakestFoe == null || card.getBattlePoints() < weakestFoe.getBattlePoints())
				{
					if (previousStageCard == null || previousStageCard.getBattlePoints() < card.getBattlePoints())
					{
						weakestFoe = card;
					}
				}
			}
		}
		return weakestFoe;
	}

	public Adventure GetStrongestFoe(Dictionary<Adventure, bool> cards) {
		Debug.Log ("Getting strongest foe");
		Logger.getInstance().info ("Getting strongest foe");
		Adventure strongestFoe = null;
		foreach (Adventure card in cards.Keys) {
			if (card.IsFoe()) {
				Debug.Log ("Found a foe");
				Logger.getInstance().info ("Found a foe");
				if (strongestFoe == null || card.getBattlePoints () > strongestFoe.getBattlePoints ()) {
					if (!cards [card]) {
						Debug.Log ("Replaced strongest foe with: " + card.GetCardName ());
						Logger.getInstance().info ("Replaced strongest foe with: " + card.GetCardName ());
                        Adventure tempcard = card;
                        strongestFoe = tempcard;
					}
				}
			}
		}
		return strongestFoe;
	}
}
