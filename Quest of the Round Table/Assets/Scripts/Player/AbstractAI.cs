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

    public abstract void PlayQuestStage(Stage stage);

    protected abstract void PlayTestStage(Stage stage);

    protected abstract void PlayFoeStage(Stage stage);

    protected abstract bool CanPlayCardForStage(Card card, List<Card> participationList);

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
            if (card.GetType().IsSubclassOf(typeof(Foe))) {
                validCards.Add(card);
                validCardCount++;
                uniqueBattlePoints.Add(((Foe)card).getBattlePoints());
            }
            else if (card.GetType().IsSubclassOf(typeof(Test))) {
                if (!ContainsTest(validCards)) {
                    validCardCount++;
                }
                validCards.Add(card);
            } else if (card.GetType().IsSubclassOf(typeof(Weapon))) {
                uniqueWeapons.Add((Weapon)card);
            }
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
                    Debug.Log(strategyOwner.getName() + " has sufficient cards (without using a test) to sponsor this quest.");
                    return (uniqueBattlePoints.Count >= quest.getNumStages());
                } else {
                    Debug.Log(strategyOwner.getName() + " has sufficient cards (using a test) to sponsor this quest.");
                    return (uniqueBattlePoints.Count >= (quest.getNumStages() - 1));
                }
            }
        }
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
            if (card.GetType().IsSubclassOf(typeof(Test))) {
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
            //if (card.GetType().IsSubclassOf(typeof(Foe))) {
            //    totalBattlePoints += ((Foe)card).getBattlePoints();
            //} else if (card.GetType().IsSubclassOf(typeof(Weapon))) {
            //    totalBattlePoints += ((Weapon)card).getBattlePoints();
            //} else if (card.GetType() == typeof(Amour)) {
            //    totalBattlePoints += ((Amour)card).getBattlePoints();
            //} else if (card.GetType().IsSubclassOf(typeof(Ally))) {
            //    totalBattlePoints += ((Ally)card).getBattlePoints();
            //}
        }
        return totalBattlePoints;
    }

	protected List<Card> SortBattlePointsCards(List<Card> cards) {
		Amour amour = null;
		List<Ally> allies = new List<Ally>();
		List<Weapon> weapons = new List<Weapon>();
		List<Card> sortedList = new List<Card>();
		Debug.Log("Available cards:");
		foreach (Card card in cards) {
			Debug.Log(card.getCardName());
		}
		Debug.Log("Looping through cards");
		foreach (Card card in cards)
		{
			bool inserted = false;
			if (card.GetType() == typeof(Amour))
			{
				amour = (Amour)card;
			}
			else if (card.GetType().IsSubclassOf(typeof(Ally)))
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
			else if (card.GetType().IsSubclassOf(typeof(Weapon)))
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
		foreach (Card card in sortedList) {
			Debug.Log(card.getCardName());
		}
		return sortedList;
	}

    protected Weapon GetBestUniqueWeapon(List<Card> cards, List<Card> currentWeapons) {
        Weapon bestWeapon = null;
        foreach (Card card in cards) {
            if (card.GetType().IsSubclassOf(typeof(Weapon))) {
                if (!currentWeapons.Contains(card)) {
                    if (bestWeapon == null || ((Weapon)card).getBattlePoints() > bestWeapon.getBattlePoints()) {
                        bestWeapon = (Weapon)card;
                    }
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
            if (card.GetType().IsSubclassOf(typeof(Foe))) {
                if (((Foe)card).getBattlePoints() < discardableCardsThreshold) {
                    discardableCards++;
                }
            }
        }
        Debug.Log(strategyOwner.getName() + " has " + discardableCards + " discardable cards.");
        return (discardableCards > 1);
    }

    public int GetTotalAvailableFoeBids()
    {
        int availableBids = 0;
        foreach (Card card in strategyOwner.getHand())
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
        foreach (Card card in strategyOwner.getHand())
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
        List<Card> TempHand = new List<Card>(strategyOwner.getHand());

        foreach (Card card in TempHand)
        {
            if (card.GetType().IsSubclassOf(typeof(Foe)))
            {
                strategyOwner.RemoveCard(card);
            }
        }
    }

    public void RemoveFoeAndDuplicateCards()
    {
        List<String> Seen = new List<String>();
        List<Card> TempHand = new List<Card>(strategyOwner.getHand());

        RemoveFoeCards();

        foreach (Card card in TempHand)
        {
            if (!Seen.Contains(card.getCardName()))
            {
                Seen.Add(card.getCardName());
            }
            else
            {
                strategyOwner.RemoveCard(card);
            }
        }
    }
}
