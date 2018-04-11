using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class KingsCallToArms : Events {

	public static int frequency = 1;
	private List<Player> highestRankPlayers; 
	public Player currentPlayer;
    public Player firstPlayer;
    private BoardManagerMediator board;

	public KingsCallToArms () : base ("King's Call to Arms") { }

	//Event description: The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded.
	public override void startBehaviour() {
		Logger.getInstance ().info ("Started the Kings Call To Arms behaviour");
        Debug.Log("Started the Kings Call To Arms behaviour");

        board = BoardManagerMediator.getInstance();
		List<Player> allPlayers = board.getPlayers();


		//Find highest ranked player(s)
		highestRankPlayers = new List<Player>();

		//populate a list of players with the lowest rank
		foreach (Player player in allPlayers) {
			if (highestRankPlayers.Count == 0) {
				highestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() > highestRankPlayers [0].getRank ().getBattlePoints()) {
				highestRankPlayers.Clear ();
				highestRankPlayers.Add (player);
			} else if (player.getRank ().getBattlePoints() == highestRankPlayers [0].getRank ().getBattlePoints()) {
				highestRankPlayers.Add (player);
			}
		}

		Logger.getInstance ().debug ("Populated a list of players with the highest rank");
        Debug.Log("Populated a list of players with the highest rank");

		if (highestRankPlayers.Count != 0) { // For safety
			currentPlayer = highestRankPlayers [0];
            firstPlayer = currentPlayer;
			Logger.getInstance ().debug ("Prompting event action for player " + currentPlayer.getName());
            Debug.Log("Prompting event action for player " + currentPlayer.getName());
			PromptEventAction ();
		}
	}

	public void PromptEventAction() {
		int numFoeCards = getNumFoeCards ();
		Logger.getInstance ().trace ("numFoeCards for the current player is " + numFoeCards);
        Debug.Log("numFoeCards for the current player is " + numFoeCards);

		if (hasWeapons ()) {
			Logger.getInstance ().debug ("Player to discard weapon");
            Debug.Log("Player to discard weapon");
			currentPlayer.PromptDiscardWeaponKingsCallToArms (this);
		} 
		else if (numFoeCards > 1) {
			Logger.getInstance ().debug ("Player to discard foes.");
            Debug.Log("Player to discard foes");
			currentPlayer.PromptDiscardFoesKingsCallToArms (this, numFoeCards);
		}
		else {
			//call same function on next player
			Logger.getInstance ().debug ("Player is unable to discard weapons/foes, moving onto next player");
            Debug.Log("Player is unable to discard weapons/foes, moving onto next player");

            if (board.IsOnlineGame())
            {
                board.getPhotonView().RPC("CallToArmsPromptNextPlayer", PhotonTargets.Others);
            }
            PromptNextPlayer();
		}
		Debug.Log ("Finished King's Call To Arms.");
	}

    public void PromptNextPlayer()
    {
        currentPlayer = board.getNextPlayer(currentPlayer);

        if (currentPlayer != firstPlayer)
        {
            Debug.Log("Got finished response from last player, moving onto next player...");
            Logger.getInstance().debug("Got finished response from last player, moving onto next player...");
            PromptEventAction();
        }
        else
        {
            board.nextTurn();
        }
    }

    public void PlayerDiscardedWeapon()
    {
        Debug.Log("Entered 'PlayerDiscardedWeapon");
        Debug.Log("PLAYER WHOSE CARD WE ARE ABOUT TO REMOVE: " + currentPlayer.getName());
        List<Adventure> dicardedCards = board.GetDiscardedCards(currentPlayer);
        Debug.Log("Number of cards discarded: " + dicardedCards.Count);

        if (dicardedCards.Count == 1){
			if (dicardedCards[0].IsWeapon()) {
				currentPlayer.GetAndRemoveCards ();

                if (board.IsOnlineGame())
                {
                    board.getPhotonView().RPC("CallToArmsPromptNextPlayer", PhotonTargets.Others);
                }
                PromptNextPlayer();
            }
            else{
                
                Debug.Log("Player played incorrect card...");
                Logger.getInstance().debug("Player played incorrect card...");
				currentPlayer.PromptDiscardWeaponKingsCallToArms (this);
            }
        }

        else{
            Debug.Log("Player discarded incorrect number of cards...");
            Logger.getInstance().debug("Player discarded incorrect number of cards...");
			currentPlayer.PromptDiscardWeaponKingsCallToArms (this);
        }
    }


    public void PlayerDiscardedFoes()
    {
        bool valid = true;
        Debug.Log("Entered 'PLayerDiscardedFoes");
        List<Adventure> dicardedCards = board.GetDiscardedCards(currentPlayer);
        Debug.Log("Number of cards discarded: " + dicardedCards.Count);

        if (dicardedCards.Count == getNumFoeCards()) {
            foreach (Card card in dicardedCards) {
				if (!card.IsFoe()) {
                    valid = false;
                }
            }

            if (valid) {
				currentPlayer.GetAndRemoveCards ();
                if (board.IsOnlineGame())
                {
                    board.getPhotonView().RPC("CallToArmsPromptNextPlayer", PhotonTargets.Others);
                }
                PromptNextPlayer();
            }

            else {
                Debug.Log("Player played incorrect card...");
                Logger.getInstance().debug("Player played incorrect card...");
				currentPlayer.PromptDiscardFoesKingsCallToArms (this, getNumFoeCards ());
            }
        }

        else{
            Debug.Log("Player discarded incorrect number of cards...");
            Logger.getInstance().debug("Player discarded incorrect number of cards...");
			currentPlayer.PromptDiscardFoesKingsCallToArms (this, getNumFoeCards ());
        }
    }


	private int getNumFoeCards() {
		int numFoeCards = 0;
		foreach (Card card in currentPlayer.GetHand()) {
			if (card.IsFoe()) {
				numFoeCards++;
				if (numFoeCards == 2) {
					return numFoeCards;
				}
			}
		}
		return numFoeCards;
	}

	public bool hasWeapons() {
		foreach (Card card in currentPlayer.GetHand()) {
			if (card.IsWeapon()) {
				return true;
			}
		}
		return false;
	}
}
