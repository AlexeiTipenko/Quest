using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player
{

	private string name;
	private int numShields;
	private Rank rank;
	private List<Card> hand;
	private PlayerPlayArea playArea;
    private BoardManagerMediator board;
	private bool isAI;
    public Action func;

	public Player(string name, bool isAI) {
		this.name = name;
		this.isAI = isAI;
		rank = new Squire ();
		numShields = 0;
		hand = new List<Card> ();
		playArea = new PlayerPlayArea ();
        board = BoardManagerMediator.getInstance();
	}

	public void dealCards(List<Card> cards) {
		foreach (Card card in cards) {
			card.setOwner (this);
			hand.Add (card);
		}
        checkNumCards();
	}

    //public void dealCard(Card card, Action func)
    //{
    //    this.func = func;
    //    card.setOwner(this);
    //    hand.Add(card);
    //    checkNumCards();
    //}

    public void checkNumCards(){
        if (hand.Count > 12)
        {
            Debug.Log("MORE THAN 12 CARDS IN HAND");
            board.PromptCardRemoveSelection(this, func);
        }
    }

    public void giveAction(Action action) {
        func = action;
    }

    public void PromptNextPlayer(){
        if (func != null){
            func();
            func = null;
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

    public int getBattlePoints() {
        return rank.getBattlePoints() + playArea.getBattlePoints();
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
			availableBids += ((Adventure)card).getBidPoints (); 
            //TODO: Make sure empowered bid points work (and battle points while you're at it!)
		}
		return availableBids;
	}

    public void RemoveCard(Card card)
    {
        //var value = MyList.First(item => item.name == "foo").value;
        //Card cardToRemove = hand.Find(c => c.getCardName() == card.getCardName());
        //Card cardToRemove = hand.First(c => c.getCardName() == card.getCardName());

        for (int i = 0; i < hand.Count(); i++)
        {
            if (card.getCardName() == hand[i].getCardName())
            {
                hand.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveCardsResponse()
    {
        board.TransferFromHandToPlayArea(this);
        List<Card> chosenCards = board.GetDiscardedCards(this);

        foreach (Card card in chosenCards)
        {
            RemoveCard(card);
            //playArea.addCard(card);
        } 

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
        BoardManagerMediator.getInstance().DrawRank(this);
		return this; //returns the player object for cascading for testing
	}

	public bool acceptQuest(){
		//prompt user if they want to sponsor quest
		//UI has to be implemented here to actually 
		//cycle through players and prompt them
		return true;
	}

	public string toString() {
		string output = this.name + " (" + (isAI ? "AI" : "human") + "): ";
		foreach (Card card in hand) {
			output += card.toString() + ", ";
		}
		output = output.Substring (0, output.Length - 2);
		return output;
	}
}


