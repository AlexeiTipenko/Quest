using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerPlayArea {

	List<Card> cards;

	public PlayerPlayArea() {

	}

	public List<Card> getCards() {
		return cards;
	}

	public void addCard(Card card) {
		cards.Add (card);
	}

	public void discardWeapons() {
		foreach (Card card in cards) {
			if (card.GetType ().IsSubclassOf (typeof(Weapon))) {
				cards.Remove (card);
			}
		}
	}

	public void discardAmours() {
		foreach (Card card in cards) {
			if (card.GetType ().IsSubclassOf (typeof(Amour))) {
				cards.Remove (card);
			}
		}
	}

	public void discardAllies() {
		foreach (Card card in cards) {
			if (card.GetType ().IsSubclassOf (typeof(Ally))) {
				cards.Remove (card);
			}
		}
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
}
