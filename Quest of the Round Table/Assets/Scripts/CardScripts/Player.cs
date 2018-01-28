using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	private string name;
	private int shieldsNum;
	private List<Card> cardsList;


	public Player(string name) {
		this.name = name;
	}


	//Getters
	public string getPlayerName() {
		return this.name;
	}

	public int getPlayerShieldNum() {
		return this.shieldsNum;
	}

	public List<Card> getPlayerCards() {
		return this.cardsList;
	}


	//Setters
	private void setName(string name) {
		this.name = name;
	}

	private void setShieldNum(int shieldsNum) {
		this.shieldsNum = shieldsNum;
	}

	private void setCardsList(List<Card> cardsList) {
		this.cardsList = cardsList;
	}


	public void givePlayerCards(List<Card> cardList) {
		foreach (Card card in cardList) {
			cardsList.Add(card);
		}

		if (cardsList.Count > 12) {
			//do something
		}
	}

	public string displayCards() {
		string displayStr = "";

		foreach (Card card in cardsList) {
			displayStr += "Player: " + this.name + " , Card Name: " + card.getCardName();
		}
		return displayStr;
	}
}


