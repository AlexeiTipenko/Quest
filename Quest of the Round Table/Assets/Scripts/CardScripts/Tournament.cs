using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tournament : Story {

	public int bonusShields;
	public int playersEntered;

	public Tournament() {
		bonusShields = 0;
		playersEntered = 0;
	}

	void enterTournament(){ //IDK how to do this properly but basically increment players enter
		playersEntered++;
	}

	int getNumShieldsWon() {
		return playersEntered + bonusShields;
	}
}
