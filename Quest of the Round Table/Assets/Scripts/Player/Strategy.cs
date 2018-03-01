using UnityEngine;
using System.Collections.Generic;

public abstract class Strategy {
    
    protected readonly BoardManagerMediator board;
    protected AIPlayer strategyOwner;
    protected int minimumFinalStageBattlePoints, discardableCardsThreshold;

    protected Strategy(int minimumFinalStageBattlePoints, int discardableCardsThreshold) {
        board = BoardManagerMediator.getInstance();
        this.minimumFinalStageBattlePoints = minimumFinalStageBattlePoints;
        this.discardableCardsThreshold = discardableCardsThreshold;
    }
    
    public abstract bool DoIParticipateInTournament();

    public abstract bool DoISponsorAQuest();

    public abstract bool DoIParticipateInQuest();

    public abstract void NextBid();

    public abstract void DiscardAfterWinningTest();

    public abstract void SponsorQuest();

    public abstract void ParticipateInQuest();

    public abstract void ParticipateTournament();

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
}
