using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : Story {

	protected int numStages;
	private enum dominantFoe {};
	private List<Stage> stages;

	public Quest (string cardName, int numStages/*, enum dominantFoe*/) : base (cardName) {
		this.numStages = numStages;
		/*this.dominantFoe = dominantFoe;*/
	}

	public int getShieldsWon() {
		return numStages;
	}

	public override void startBehaviour(){

		GameObject boardManager = GameObject.Find("BoardManager");
		BoardManager boardScripts = boardManager.GetComponent<BoardManager> ();
		List<Player> allPlayers = boardScripts.getPlayers();


		Debug.Log ("Quest behaviour started");

		foreach (Player player in allPlayers) {
			if (player.acceptQuest ()) {		
				if (sponsorValid (player)) {

					// game continues
					// 3) Player places desired foes onto board???
					// 4) Player clicks "Ready button"
					// 5) card validation

				}
			} 
		}
			

		// 6) loop through players to see if they pass each round (with only their current rank points)

		 
	}

	private bool sponsorValid(Player player){
		
		//validate if current player has needed cards.
		List<Card> hand = player.getHand();

		int foeCount = 0;
		foreach (Card card in hand) {
			//if (card.GetType().subclass == Quest)
			//foeCount++;
		}

		if (foeCount >= numStages)
			return true;
		
		else
			return false;
	}
}
