using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	private string name;
	private int numShields;
	private Rank rank;
	private List<Card> hand;
	private PlayerPlayArea playArea;
	private bool isAI;

	public Player(string name, bool isAI) {
		this.name = name;
		this.isAI = isAI;
		rank = new Squire ();
		numShields = 0;
		hand = new List<Card> ();
		playArea = new PlayerPlayArea ();
	}

	public void dealCards(List<Card> cards) {
		foreach (Card card in cards) {
			card.setOwner (this);
			hand.Add (card);
		}

		if (hand.Count > 12) {
			//prompt player to discard cards
		}
	}

	private void checkForRankUp() {
		if (numShields == rank.getShieldsToProgress ()) {
			numShields = 0;
			upgradeRank ();
			//numShields = numShields - rank.getShieldsToProgress ();
			Debug.Log ("Rank is now: " + getRank ());
		} else if (numShields > rank.getShieldsToProgress ()) {
			numShields -= rank.getShieldsToProgress ();
			upgradeRank ();
			Debug.Log ("Rank is now: " + getRank ());
		}
	}



	public string getName() {
		return this.name;
	}

	public int getNumShields() {
		return this.numShields;
	}

	public List<Card> getHand() {
		return this.hand;
	}

	public PlayerPlayArea getPlayArea () {
		return this.playArea;
	}

	public Rank getRank() {
		return this.rank;
	}

	public int getTotalAvailableBids() {
		int availableBids = hand.Count;
		List<Card> playAreaCards = playArea.getCards ();
		foreach (Card card in playAreaCards) {
			availableBids += ((Adventure)card).getBidPoints (); //TODO: Make sure empowered bid points work (and battle points while you're at it!)
		}
		return availableBids;
	}

	public void incrementShields(int numShields) {
		this.numShields += numShields;
		checkForRankUp ();
	}

	public void decrementShields(int numShields) {
		if (this.numShields-numShields < 0) {
			this.numShields = 0;
		} else {
			this.numShields -= numShields;
		}
	}

	public Player upgradeRank(){
		rank = rank.upgrade ();
        BoardManager.DestroyRank();
        BoardManager.DrawRank(this);
		return this; //returns the player object for cascading for testing
	}

	public bool acceptQuest(){
		//prompt user if they want to sponsor quest
		//UI has to be implemented here to actually 
		//cycle through players and prmpt them
		return true;
	}

	public string toString() {
		string toString = this.name + " (" + (isAI ? "AI" : "human") + "): ";
		foreach (Card card in hand) {
			toString += card.toString() + ", ";
		}
		toString = toString.Substring (0, toString.Length - 2);
		return toString;
	}
}


