using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tournament : Story {

	protected int bonusShields;
	protected int playersEntered;

	public Tournament(string cardName, int bonusShields): base(cardName) {
		this.playersEntered = 0;
		this.bonusShields = bonusShields;
	}
		
	public int getBonusShields() {
		return bonusShields;
	}
	/*
	void enterTournament(){ //IDK how to do this properly but basically increment players enter
		playersEntered++;
	}

	int getNumShieldsWon() {
		return playersEntered + bonusShields;
	}
	*/
}
