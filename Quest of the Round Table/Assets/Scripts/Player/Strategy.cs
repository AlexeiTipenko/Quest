using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Strategy {
    
    protected readonly BoardManagerMediator board;
    protected AIPlayer strategyOwner;
    protected int minimumFinalStageBattlePoints, discardableCardsThreshold;

    protected Strategy(int minimumFinalStageBattlePoints, int discardableCardsThreshold) {
        board = BoardManagerMediator.getInstance();
        this.minimumFinalStageBattlePoints = minimumFinalStageBattlePoints;
        this.discardableCardsThreshold = discardableCardsThreshold;
    }
    
    public abstract void DoIParticipateInTournament();

    public abstract bool DoISponsorAQuest();

    public abstract bool DoIParticipateInQuest();

    public abstract void NextBid();

    public abstract void DiscardAfterWinningTest();

    public abstract void SponsorQuest();

    public abstract void PlayQuestStage(Stage stage);

    protected abstract void PlayTestStage(Stage stage);

    protected abstract void PlayFoeStage(Stage stage);

    protected abstract bool CanPlayCardForStage(Card card, List<Card> participationList);

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
            if (card.GetType().IsSubclassOf(typeof(Foe))) {
                totalBattlePoints += ((Foe)card).getBattlePoints();
            } else if (card.GetType().IsSubclassOf(typeof(Weapon))) {
                totalBattlePoints += ((Weapon)card).getBattlePoints();
            } else if (card.GetType() == typeof(Amour)) {
                totalBattlePoints += ((Amour)card).getBattlePoints();
            } else if (card.GetType().IsSubclassOf(typeof(Ally))) {
                totalBattlePoints += ((Ally)card).getBattlePoints();
            }
        }
        return totalBattlePoints;
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
		
	public void discardLowestWeaponCard() {
		List<Card> cardsInHand = strategyOwner.getHand ();

		List<Weapon> weapons = new List<Weapon> ();

		foreach (Card card in cardsInHand) {
			if (card.GetType().IsSubclassOf(typeof(Weapon))) {
				weapons.Add ((Weapon)card);
			}
		}

		if (weapons.Count == 0) {
			Debug.LogError ("There are 0 of the type of the weapons that was passed in");
			Logger.getInstance ().error ("There are 0 of the type of the weapons that was passed in");
			return;
		}

		Weapon lowestPointWeapon = weapons [0];

		for (int i = 1; i < weapons.Count; i++) {
			if (weapons[i].getBattlePoints() < lowestPointWeapon.getBattlePoints()) {
				lowestPointWeapon = weapons [i];
			}
		}

		strategyOwner.RemoveCard (lowestPointWeapon);
	}

	public void discardLowestFoeCard() {
		List<Card> cardsInHand = strategyOwner.getHand ();

		List<Foe> foes = new List<Foe> ();

		foreach (Card card in cardsInHand) {
			if (card.GetType().IsSubclassOf(typeof(Foe))) {
				foes.Add ((Foe)card);
			}
		}

		if (foes.Count == 0) {
			Debug.LogError ("There are 0 of the type of the foes that were passed in");
			Logger.getInstance ().error ("There are 0 of the type of the foes that were passed in");
			return;
		}

		Foe lowestPointFoe = foes [0];

		for (int i = 1; i < foes.Count; i++) {
			if (foes[i].getBattlePoints() < lowestPointFoe.getBattlePoints()) {
				lowestPointFoe = foes [i];
			}
		}

		strategyOwner.RemoveCard (lowestPointFoe);
	}

}
