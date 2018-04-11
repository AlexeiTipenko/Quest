using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Card {

	protected Player owner;
	protected string cardName;
	public string cardImageName;

	protected Card(string cardName) {
        Logger.getInstance ().info("Creating card: " + cardName);
		this.cardName = cardName;
		this.owner = null;
	}

	public bool IsAlly() {
		return (GetType ().IsSubclassOf (typeof(Ally)));
	}

	public bool IsWeapon() {
		return (GetType ().IsSubclassOf (typeof(Weapon)));
	}

	public bool IsAmour() {
		return (GetType () == typeof(Amour));
	}

	public bool IsFoe() {
		return (GetType ().IsSubclassOf (typeof(Foe)));
	}

	public bool IsTest() {
		return (GetType ().IsSubclassOf (typeof(Test)));
	}

	public bool IsQuest() {
		return (GetType ().IsSubclassOf (typeof(Quest)));
	}

	public bool IsAdventure() {
		return (GetType ().IsSubclassOf (typeof(Adventure)));
	}

	public bool IsStory() {
		return (GetType ().IsSubclassOf (typeof(Story)));
	}

	//Getters
	public Player GetOwner() {
		return this.owner;
	}

	public string GetCardName() {
		return this.cardName;
	}


	//Setters
	public void SetOwner(Player owner) {
		this.owner = owner;
	}

	public void SetCardImageName(string name) {
		cardImageName = name;
	}

	public string ToString() {
		return cardName + " (" + owner.getName() + ")";
	}
}
