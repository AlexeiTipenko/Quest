using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tournament : Story {

	protected int bonusShields;
	protected int playersEntered;

	public Tournament() : base() {
		this.bonusShields = 0;
		this.playersEntered = 0;
	}

	public Tournament(Player owner, string cardName): base(owner, cardName) {
		this.playersEntered = 0;
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
