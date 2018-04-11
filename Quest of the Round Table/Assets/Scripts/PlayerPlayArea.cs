using System.Collections.Generic;
using System;

[Serializable]
public class PlayerPlayArea {

	List<Adventure> cards;

	public PlayerPlayArea() {
        cards = new List<Adventure>();
	}

	public List<Adventure> getCards() {
		return cards;
	}


    public int getBattlePoints() {
        int totalPoints = 0;
        foreach(Adventure card in cards){
            totalPoints += card.getBattlePoints();
        }
        return totalPoints;
    }


	public void addCard(Adventure card) {
		cards.Add (card);
	}

	public void discardWeapons() {
        List<Adventure> tempCards = new List<Adventure>();
		foreach (Adventure card in cards) {
            if (!card.IsWeapon()) {
                tempCards.Add(card);
            }
		}
        cards = tempCards;
	}

	public void discardAmours() {
        List<Adventure> tempCards = new List<Adventure>();
        foreach (Adventure card in cards)
        {
			if (!card.IsAmour())
            {
                tempCards.Add(card);
            }
        }
        cards = tempCards;
	}

	public void discardAllies() {
        List<Adventure> tempCards = new List<Adventure>();
        foreach (Adventure card in cards)
        {
            if (!card.IsAlly())
            {
                tempCards.Add(card);
            }
        }
        cards = tempCards;
	}

	public void discardAlly(Type type) {
		foreach (Adventure card in cards) {
			if (card.GetType () == type) {
				cards.Remove (card);
				break;
			}
		}
	}

    public bool containsCard(String cardName) {
        foreach (Adventure card in cards) {
            if (card.GetCardName() == cardName) {
                return true;
            }
        }
        return false;
    }
}
