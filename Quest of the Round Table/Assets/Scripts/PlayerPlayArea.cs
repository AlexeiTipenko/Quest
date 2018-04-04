using System.Collections.Generic;
using System;

[Serializable]
public class PlayerPlayArea {

	List<Card> cards;

	public PlayerPlayArea() {
        cards = new List<Card>();
	}

	public List<Card> getCards() {
		return cards;
	}


    public int getBattlePoints() {
        int totalPoints = 0;
        /*GameObject boardArea = GameObject.Find("Canvas/TabletopImage/BoardArea");
        int totalPoints = 0;

        foreach (Transform child in boardArea.transform)
        {
            Card tempCard = cards.Find(c => c.getCardName() == child.gameObject.name);
            Adventure card = (Adventure)tempCard;
            totalPoints += card.getBattlePoints();
        }*/

        foreach(Adventure card in cards){
            totalPoints += card.getBattlePoints();
        }
        return totalPoints;
    }


	public void addCard(Card card) {
		cards.Add (card);
	}

	public void discardWeapons() {
        List<Card> tempCards = new List<Card>();
		foreach (Card card in cards) {
            if (!card.GetType().IsSubclassOf(typeof(Weapon))) {
                tempCards.Add(card);
            }
		}
        cards = tempCards;
	}

	public void discardAmours() {
        List<Card> tempCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (!card.GetType().Equals(typeof(Amour)))
            {
                tempCards.Add(card);
            }
        }
        cards = tempCards;
	}

	public void discardAllies() {
        List<Card> tempCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (!card.GetType().IsSubclassOf(typeof(Ally)))
            {
                tempCards.Add(card);
            }
        }
        cards = tempCards;
	}

	public void discardAlly(Type type) {
		//TODO: maybe this can be implemented by specifically referencing the card to be discarded?
		foreach (Card card in cards) {
			if (card.GetType () == type) {
				cards.Remove (card);
				break;
			}
		}
	}

    public bool containsCard(String cardName) {
        foreach (Card card in cards) {
            if (card.getCardName() == cardName) {
                return true;
            }
        }
        return false;
    }
}
