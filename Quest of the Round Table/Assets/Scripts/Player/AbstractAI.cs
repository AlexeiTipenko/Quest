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

    public abstract List<Card> ParticipateTournament();

    //--------------------------------------------//
    //---------- Non-Abstract Functions ----------//
    //--------------------------------------------//

    public void SetStrategyOwner(AIPlayer strategyOwner) {
        this.strategyOwner = strategyOwner;
    }

    protected bool SomeoneElseCanWinOrEvolveWithQuest() {
        Debug.Log("Checking if someone else can win or evolve through this quest.");
        Quest quest = (Quest)board.getCardInPlay();
        foreach (Player player in board.getPlayers()) {
            if (player != strategyOwner) {
                if (player.getNumShields() + quest.getNumStages() >= player.getRank().getShieldsToProgress()) {
                    Debug.Log("Player " + player.getName() + " can win off this quest.");
                    return true;
                }
            }
        }
        Debug.Log("No player can win off this quest.");
        return false;
    }

    protected bool SufficientCardsToSponsorQuest() {
        Debug.Log("Checking if " + strategyOwner.getName() + " has sufficiently valid cards to sponsor quest.");
        Quest quest = (Quest)board.getCardInPlay();
        List<Card> cards = strategyOwner.getHand();
        List<Card> validCards = new List<Card>();
        HashSet<int> uniqueBattlePoints = new HashSet<int>();
        HashSet<Weapon> uniqueWeapons = new HashSet<Weapon>();
        int validCardCount = 0;
        foreach (Card card in cards) {
			if (card.IsFoe()) {
                validCards.Add(card);
                validCardCount++;
                uniqueBattlePoints.Add(((Foe)card).getBattlePoints());
            }
			else if (card.IsTest()) {
                if (!ContainsTest(validCards)) {
                    validCardCount++;
                }
                validCards.Add(card);
			} else if (card.IsWeapon()) {
				bool unique = true;
				foreach (Weapon weapon in uniqueWeapons) {
					if (weapon.GetCardName () == card.GetCardName ()) {
						unique = false;
						break;
					}
				}
				if (unique) {
					uniqueWeapons.Add((Weapon)card);
				}
            }
        }
		foreach (Card card in uniqueWeapons) {
			Debug.Log("Unique weapons: " + card.GetCardName());
		}
        if (validCardCount >= quest.getNumStages()) {
            Debug.Log(strategyOwner.getName() + " has enough valid cards.");
            int totalWeaponsBattlePoints = 0;
            foreach (Weapon weapon in uniqueWeapons) {
                totalWeaponsBattlePoints += weapon.getBattlePoints();
            }
            if (totalWeaponsBattlePoints + HighestBattlePoints(uniqueBattlePoints) >= minimumFinalStageBattlePoints) {
                Debug.Log(strategyOwner.getName() + " has sufficient cards to meet minimumFinalStageBattlePoints requirement.");
                if (!ContainsTest(validCards)) {
					if (uniqueBattlePoints.Count >= quest.getNumStages ()) {
						Debug.Log(strategyOwner.getName() + " has sufficient cards (without using a test) to sponsor this quest.");
						return true;
					}
                } else {
					if (uniqueBattlePoints.Count >= (quest.getNumStages () - 1)) {
						Debug.Log(strategyOwner.getName() + " has sufficient cards (using a test) to sponsor this quest.");
						return true;
					}
                }
            }
        }
		Debug.Log ("Didn't meet battlepoint requirements");
        return false;
    }

	protected bool SomeoneElseCanWinOrEvolveWithTournament(List<Player> players) {
		Debug.Log("Checking if someone else can win or evolve through this tournament.");
        Logger.getInstance().info("AI Strategy 1 checking if someone else can win/evolve through tournament");
		Tournament tournament = (Tournament)board.getCardInPlay();
		foreach (Player player in players) {
			if (player != strategyOwner) {
				if (player.getNumShields() + tournament.GetBonusShields() >= player.getRank().getShieldsToProgress()) {
					Debug.Log("Player " + player.getName() + " can win off this tournament.");
                    Logger.getInstance().info("AI Strategy 1 found that Player: " + player.getName() + " can win off this tournament.");
					return true;
				}
			}
		}
		Debug.Log("No player can win off this tournament.");
		return false;
	}

    protected bool ContainsTest(List<Card> cards) {
        foreach (Card card in cards) {
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

    protected int GetTotalBattlePoints(List<Card> cards) {
        int totalBattlePoints = 0;
        foreach (Card card in cards) {
            totalBattlePoints += ((Adventure)card).getBattlePoints();
        }
        return totalBattlePoints;
    }

	protected List<Adventure> SortCardsByType(List<Card> cards) {
		Amour amour = null;
		List<Ally> allies = new List<Ally>();
		List<Weapon> weapons = new List<Weapon>();
		List<Adventure> sortedList = new List<Adventure>();
		Debug.Log("Available cards:");
		foreach (Card card in cards) {
			Debug.Log(card.GetCardName());
		}
		Debug.Log("Looping through cards");
		foreach (Card card in cards)
		{
			bool inserted = false;
			if (card.GetType() == typeof(Amour))
			{
				amour = (Amour)card;
			}
			else if (card.IsAlly())
			{
				List<Ally> tempAllies = new List<Ally>(allies);
				foreach (Ally ally in allies)
				{
					if (((Ally)card).getBattlePoints() <= ally.getBattlePoints())
					{
						tempAllies.Insert(tempAllies.IndexOf(ally), (Ally)card);
						inserted = true;
						break;
					}
				}
				if (!inserted) {
					tempAllies.Add((Ally)card);
				}
				allies = new List<Ally>(tempAllies);
			}
			else if (card.IsWeapon())
			{
				List<Weapon> tempWeapons = new List<Weapon>(weapons);
				foreach (Weapon weapon in weapons)
				{
					if (((Weapon)card).getBattlePoints() <= weapon.getBattlePoints())
					{
						tempWeapons.Insert(tempWeapons.IndexOf(weapon), (Weapon)card);
						inserted = true;
						break;
					}
				}
				if (!inserted) {
					tempWeapons.Add((Weapon)card);
				}
				weapons = new List<Weapon>(tempWeapons);
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
		foreach (Adventure card in sortedList) {
			Debug.Log(card.GetCardName());
		}
		return sortedList;
	}

	protected List<Adventure> SortCardsByBattlePoints(List<Card> cards) {
		List<Adventure> sortedCards = new List<Adventure> ();
		bool containsAmour = false;
		foreach (Card card in cards) {
			if (card.IsAlly() || card.IsWeapon() || card.GetType() == typeof(Amour)) {
				int index = -1;
				foreach (Adventure sortedCard in sortedCards) {
					if (((Adventure)card).getBattlePoints () > sortedCard.getBattlePoints ()) {
						index = sortedCards.IndexOf (sortedCard);
					}
				}
				InsertIntoListAtIndex ((Adventure)card, sortedCards, index, containsAmour);
				if (card.GetType () == typeof(Amour)) {
					containsAmour = true;
				}
			}
		}
		return sortedCards;
	}

	private List<Adventure> InsertIntoListAtIndex(Adventure card, List<Adventure> sortedCards, int index, bool containsAmour) {
		if (card.GetType() == typeof(Amour) && containsAmour) {
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
		else if (card.IsWeapon()) {
			foreach (Card participationCard in participationList) {
				if (participationCard.GetType() == card.GetType()) {
					return false;
				}
			}
		}
		return true;
	}

    protected Weapon GetBestUniqueWeapon(List<Card> cards, List<Card> currentWeapons) {
		Debug.Log ("Getting best unique weapon");
        Weapon bestWeapon = null;
        foreach (Card card in cards) {
			if (card.IsWeapon()) {
				Debug.Log ("Found a weapon: " + card.GetCardName ());
				bool exists = false;
				foreach (Card weapon in currentWeapons) {
					if (weapon.GetCardName () == card.GetCardName ()) {
						Debug.Log ("Weapon has already been selected");
						exists = true;
						break;
					}
				}
                if (!exists) {
					Debug.Log ("Weapon has not been selected");
                    if (bestWeapon == null || ((Weapon)card).getBattlePoints() > bestWeapon.getBattlePoints()) {
						Debug.Log ("Updating best weapon to: " + card.GetCardName ());
                        bestWeapon = (Weapon)card;
                    }
                }
            }
        }
        return bestWeapon;
    }

	protected Weapon GetBestDuplicateWeapon(Dictionary<Card, bool> cards) {
		Debug.Log ("Getting best duplicate weapon");
		Dictionary<string, List<Card>> cardLists = new Dictionary<string, List<Card>> ();
		foreach (Card card in cards.Keys) {
			if (card.IsWeapon() && !cards [card]) {
				Debug.Log ("Found an unused weapon: " + card.GetCardName ());
				string cardName = card.GetCardName ();
				List<Card> cardList = new List<Card> ();
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
				int newWeaponBattlePoints = ((Weapon)cardLists [card] [0]).getBattlePoints ();
				if (bestWeaponName == null || newWeaponBattlePoints > ((Weapon)cardLists [bestWeaponName] [0]).getBattlePoints ()) {
					Debug.Log ("Updated best duplicate weapon: " + card);
					bestWeaponName = card;
				}
			}
		}

		Weapon bestWeapon = null;
		if (bestWeaponName != null) {
			List<Card> cardList = cardLists [bestWeaponName];
			foreach (Card card in cardList) {
				if (!cards [card]) {
					Debug.Log ("Retrieved best duplicate weapon");
					bestWeapon = (Weapon)card;
					break;
				}
			}
		}
		return bestWeapon;
	}

    protected Stage InitializeStage(Card stageCard, List<Card> weapons, int stageNum) {
        Stage stage = new Stage((Adventure)stageCard, weapons, stageNum);
        strategyOwner.RemoveCard(stageCard);
        if (weapons != null) {
            foreach (Weapon weapon in weapons) {
                strategyOwner.RemoveCard(weapon);
            }
        }
        return stage;
    }

    protected bool SufficientDiscardableCards() {
        List<Card> cards = strategyOwner.getHand();
        int discardableCards = 0;
        foreach (Card card in cards) {
			if (card.IsFoe()) {
                if (((Foe)card).getBattlePoints() < discardableCardsThreshold) {
                    discardableCards++;
                }
            }
        }
        Debug.Log(strategyOwner.getName() + " has " + discardableCards + " discardable cards.");
        return (discardableCards > 1);
    }

    public void DropoutTest(int currentBid, Stage stage) {
        stage.PromptTestResponse(true, 0);
    }

    public int GetTotalAvailableFoeBids(int strategy)
    {
        if(strategy == 2){
            int availableBids = 0;
            foreach (Card card in strategyOwner.getHand())
            {
                if (card.IsFoe() && ((Foe)card).getBattlePoints() < discardableCardsThreshold)
                {
                    availableBids += 1;
                }
            }
            return availableBids;
        }
        else {
            int availableBids = 0;
            foreach (Card card in strategyOwner.getHand())
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
        foreach (Card card in strategyOwner.getHand())
        {
			if (!cardDictionary.ContainsKey(card.GetCardName()) && !card.IsFoe())
            {
                Debug.Log("Inserting into Dictionary: " + card.GetCardName());
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
                availableBids += entry.Value - 1;
            }
        }
        Debug.Log("Duplicates are: " + availableBids + " inside Foe and Dups");
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
            List<Card> TempHand = new List<Card>(strategyOwner.getHand());
            foreach (Card card in TempHand)
            {
                if (card.IsFoe() && ((Foe)card).GetMinBattlePoints() < discardableCardsThreshold)
                {
                    strategyOwner.RemoveCard(card);
                }
            }  
        }
        else {
            List<Card> TempHand = new List<Card>(strategyOwner.getHand());
            foreach (Card card in TempHand)
            {
                if (card.IsFoe() && ((Foe)card).getBattlePoints() < discardableCardsThreshold)
                {
                    strategyOwner.RemoveCard(card);
                }
            }   
        }
    }

    public void RemoveFoeAndDuplicateCards(int strategy)
    {
        List<String> Seen = new List<String>();
        List<Card> TempHand = new List<Card>(strategyOwner.getHand());

        if (strategy == 1){
			RemoveFoeCards(1);
        }
        else {
            RemoveFoeCards(2);
        }

        foreach (Card card in TempHand)
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

	public Foe GetWeakestFoe(List<Card> cards, Card previousStageCard)
	{
		Foe weakestFoe = null;
		foreach (Card card in cards)
		{
			if (card.IsFoe())
			{
				if (weakestFoe == null || ((Foe)card).getBattlePoints() < weakestFoe.getBattlePoints())
				{
					if (previousStageCard == null || ((Foe)previousStageCard).getBattlePoints() < ((Foe)card).getBattlePoints())
					{
						weakestFoe = (Foe)card;
					}
				}
			}
		}
		return weakestFoe;
	}

	public Foe GetStrongestFoe(Dictionary<Card, bool> cards) {
		Debug.Log ("Getting strongest foe");
		Foe strongestFoe = null;
		foreach (Card card in cards.Keys) {
			if (card.IsFoe()) {
				Debug.Log ("Found a foe");
				if (strongestFoe == null || ((Foe)card).getBattlePoints () > strongestFoe.getBattlePoints ()) {
					if (!cards [card]) {
						Debug.Log ("Replaced strongest foe with: " + card.GetCardName ());
                        Card tempcard = card;
                        strongestFoe = (Foe)tempcard;
					}
				}
			}
		}
		return strongestFoe;
	}
}
