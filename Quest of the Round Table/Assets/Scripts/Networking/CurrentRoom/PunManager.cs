using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PunManager : Photon.MonoBehaviour {

    BoardManagerMediator board;

	[PunRPC]
	public void UpdateSeed(int seed) {
        Debug.Log("Updated seed is: " + seed);
		Deck.seed = seed;
	}

    [PunRPC]
    public void SwitchScene(string sceneName)
    {
        Debug.Log("Calling back to player layout group");
        PlayerLayoutGroup.SwitchScene(sceneName);
    }

    [PunRPC]
    public void SwitchScene1(string sceneName){
        PlayerLayoutGroup.SwitchScene1(sceneName);
    }

	[PunRPC]
	public void TransferCards (byte[] playerBytes, byte[] cardBytes) {
		PrepareRPC ();
		Player tempPlayer = (Player)Deserialize (playerBytes);
		Card card = (Card)Deserialize (cardBytes);
		Player localPlayer = FindLocalPlayer (tempPlayer);
		if (localPlayer != null) {
			List<Card> localHand = localPlayer.getHand ();
			foreach (Card localCard in localHand) {
				if (localCard.getCardName () == card.getCardName ()) {
					BoardManager.TransferCards (localPlayer, localCard);
				}
			}
		}
	}

	[PunRPC]
	public void nextTurn () {
		PrepareRPC ();
		board.nextTurn ();
	}

    //-----------------------------------------------------------------------//
    //--------------------------- Quest Functions ---------------------------//
    //-----------------------------------------------------------------------//

	[PunRPC]
	public void RemoveCardsResponse (byte[] playerBytes, byte[] chosenCardsBytes) {
        PrepareRPC ();
        List<Card> chosenCards = (List<Card>)Deserialize(chosenCardsBytes);
        Player tempPlayer = (Player)Deserialize(playerBytes);
		Logger.getInstance ().info ("Received player: " + tempPlayer.getName());
		Player player = FindLocalPlayer (tempPlayer);
		if (player != null) {
			player.RemoveCardsResponse (chosenCards);
		}
	}

	[PunRPC]
	public void DealCardsNextPlayer() {
        PrepareRPC ();
		((Quest)board.getCardInPlay()).getCurrentStage().DealCardsNextPlayer();
	}

	[PunRPC]
	public void PromptNextPlayer() {
        PrepareRPC();
		((Tournament)board.getCardInPlay()).PromptNextPlayer();
	}

    [PunRPC]
    public void PromptSponsorQuestResponse(bool sponsorAccepted) {
        PrepareRPC();
        ((Quest)board.getCardInPlay()).PromptSponsorQuestResponse(sponsorAccepted);
    }

    [PunRPC]
    public void SponsorQuestComplete(byte[] stagesBytes) {
        PrepareRPC();
        List<Stage> stages = (List<Stage>)Deserialize(stagesBytes);
        Player sponsorPlayer = ((Quest)board.getCardInPlay()).getSponsor();
		Player player = FindLocalPlayer (sponsorPlayer);
		if (player != null) {
			((Quest)board.getCardInPlay()).SponsorQuestComplete(stages);
		}
    }

    [PunRPC]
    public void IncrementSponsor() {
        PrepareRPC();
        ((Quest)board.getCardInPlay()).IncrementSponsor();
    }

    [PunRPC]
    public void PromptAcceptQuestResponse(bool questAccepted) {
        PrepareRPC();
        ((Quest)board.getCardInPlay()).PromptAcceptQuestResponse(questAccepted);
    }

    [PunRPC]
    public void PromptNextAcceptQuest() {
        PrepareRPC ();
        ((Quest)board.getCardInPlay()).PromptNextAcceptQuest();
    }

	[PunRPC]
	public void PromptFoeResponse(bool dropOut) {
		PrepareRPC ();
		Quest quest = (Quest)board.getCardInPlay ();
		quest.getCurrentStage ().PromptFoeResponse (dropOut);
	}
		
	[PunRPC]
	public void PromptEnterTest(byte[] playerBytes, int currentBid) {
		PrepareRPC ();
		Player tempPlayer = Deserialize (playerBytes);
		Player localPlayer = FindLocalPlayer (tempPlayer);
		board.PromptEnterTest ((Quest)board.getCardInPlay (), localPlayer, currentBid);
	}

    //------------------------------------------------------------------------//
    //------------------------- Tournament Functions -------------------------//
    //------------------------------------------------------------------------//

    [PunRPC]
    public void CardsSelectionResponse()
    {
		PrepareRPC ();
        ((Tournament)board.getCardInPlay()).CardsSelectionResponse();
    }

    /*
	[PunRPC]
	public void PromptEnterTournamentResponse(bool entered)
	{
<<<<<<< HEAD
		GetBoard ();
<<<<<<< HEAD
=======
		PrepareRPC ();
>>>>>>> networking/quests
		tournament.PromptEnterTournamentResponse(entered);
	}
	*/

    [PunRPC]
    public void PromptEnterTournamentResponse(bool entered)
    {
		PrepareRPC ();
		Debug.Log ("board.cardinplay in RPC promptentertournamentresponse is " + board.getCardInPlay().getCardName());
        ((Tournament)board.getCardInPlay()).PromptEnterTournamentResponse(entered);
    }

	//---------------------------------------------------------------------//
	//------------------------- Utility Functions -------------------------//
	//---------------------------------------------------------------------//

    void PrepareRPC(){
        board = BoardManagerMediator.getInstance();
        BoardManager.ClearInteractions();
    }

	Player FindLocalPlayer(Player player) {
		List<Player> players = board.getPlayers ();
		foreach (Player localPlayer in players) {
			if (localPlayer.getName () == player.getName ()) {
				Logger.getInstance ().info ("Found matching name");
				return localPlayer;
			}
		}
		return null;
	}

    public static byte[] Serialize(System.Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static System.Object Deserialize(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

}