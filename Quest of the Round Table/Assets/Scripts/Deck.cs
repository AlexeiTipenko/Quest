using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Deck {

	static System.Random rand;
    public static int seed = 0;

	protected List<Card> cards;

    protected List<int> reservedIndices;

	public Deck() {
		if (rand == null) {
            if (seed == 0) {
                rand = new System.Random();
            } else {
                rand = new System.Random(seed);
            }
		}
		cards = new List<Card> ();
        reservedIndices = new List<int>();
	}

	public Deck(Deck oldDeck) : this () {
		foreach (Card card in oldDeck.getCards()) {
			cards.Add(card);
		}
		shuffle ();
	}

	protected void shuffle() {
		for (int i = 0; i < cards.Count - 1; i++) {
			int j = rand.Next (i, cards.Count - 1);
			Card tempCard = cards [i];
			cards [i] = cards [j];
			cards [j] = tempCard;
		}
		Debug.Log (cards [0]);
	}

	public List<Card> getCards() {
		return cards;
	}

	public int getSize() {
		return cards.Count;
	}

	public Card drawCard () {
		Card card = cards [0];
		cards.RemoveAt (0);
		card.setOwner (BoardManagerMediator.getInstance ().getCurrentPlayer ());
		return card;
	}

	protected void instantiateCards(List<string> newCards) {
		for (int i = 0; i < newCards.Count; i++) {
			Type genericType = Type.GetType(newCards[i], true);
			int frequency = (int) genericType.GetField ("frequency").GetValue(null);
			for (int j = 0; j < frequency; j++) {
				Card card = (Card)Activator.CreateInstance (genericType);
				card.setCardImageName(newCards[i]);
				cards.Add(card);
			}
		}
	}

	protected int getCardIndexByName(string cardName) {
		for (int i = 0; i < cards.Count; i++) {
			if (cards [i].cardImageName.Equals (cardName)) {
                if (!reservedIndices.Contains(i)) {
                    return i;
                }
			}
		}
		return -1;
	}

    public void moveCardToIndex(string cardName, int newIndex)
    {
        int oldIndex = getCardIndexByName(cardName);
        if (oldIndex == -1)
        {
            Debug.LogError("Card by the name " + cardName + " does not exist in deck.");
            Logger.getInstance().error("Card by the name " + cardName + " does not exist in deck.");
        }
        Card oldCard = cards[newIndex];
        Card newCard = cards[oldIndex];

        cards[newIndex] = newCard;
        cards[oldIndex] = oldCard;

        Debug.Log(newCard.getCardName() + " now at position " + newIndex);
        Debug.Log(oldCard.getCardName() + " now at position " + oldIndex);

        reservedIndices.Add(newIndex);
    }

	public virtual string toString() {
		string text = "Deck: ";
		foreach (Card card in cards) {
			text += (card.toString () + ", ");
		}
		if (cards.Count > 0) {
			text = text.Substring (0, text.Length - 2);
		}
		return text;
	}

}
