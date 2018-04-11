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


    public int GetBattlePoints() {
        int totalPoints = 0;
        foreach(Adventure card in cards){
            totalPoints += card.getBattlePoints();
        }
        return totalPoints;
    }


	public void AddCard(Adventure card) {
		cards.Add (card);
	}

	public void DiscardClass(Type type) {
		List<Adventure> tempCards = new List<Adventure> ();
		foreach (Adventure card in cards) {
			if (!card.MatchesType(type)) {
				tempCards.Add (card);
			}
		}
		cards = tempCards;
	}

	public void DiscardChosenAlly(Type type) {
		foreach (Adventure card in cards) {
			if (card.GetType () == type) {
				cards.Remove (card);
				break;
			}
		}
	}

    public bool Contains(String cardName) {
        foreach (Adventure card in cards) {
            if (card.GetCardName() == cardName) {
                return true;
            }
        }
        return false;
    }
}
