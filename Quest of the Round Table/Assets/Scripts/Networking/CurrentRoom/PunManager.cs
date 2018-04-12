using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PunManager : Photon.MonoBehaviour {

    BoardManagerMediator board;

    [PunRPC]
	public void AddAI(int aiNum, int aiID) {
        Logger.getInstance().info("Updating AI for other players");
		GameObject obj = GameObject.Find ("Canvas/CurrentRoom/PlayerList/Viewport/PlayerLayoutGroup");
		PlayerLayoutGroup playerLayoutGroup = obj.GetComponent<PlayerLayoutGroup> ();
		playerLayoutGroup.AddAI(aiNum, aiID);
    }

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
	public void SwitchSceneScenario(string sceneName, int scenarioNum) {
		PlayerLayoutGroup.SwitchSceneScenario(sceneName, scenarioNum);
    }

	[PunRPC]
	public void TransferCard (byte[] playerBytes, byte[] cardBytes) {
		board = BoardManagerMediator.getInstance ();
		Player tempPlayer = (Player)Deserialize (playerBytes);
		Adventure card = (Adventure)Deserialize (cardBytes);
		Player localPlayer = FindLocalPlayer (tempPlayer);
		if (localPlayer != null) {
			List<Adventure> localHand = localPlayer.GetHand ();
			foreach (Adventure localCard in localHand) {
				if (localCard.GetCardName () == card.GetCardName ()) {
					BoardManager.TransferCard (localPlayer, localCard);
					break;
				}
			}
		}
	}

	[PunRPC]
	public void nextTurn () {
		PrepareRPC ();
		board.nextTurn ();
	}

    [PunRPC]
    public void DiscardChosenAlly(string discardedAlly){
        PrepareRPC();
        BoardManagerMediator.getInstance().DiscardChosenAlly(discardedAlly);
    }

    //-----------------------------------------------------------------------//
    //--------------------------- Quest Functions ---------------------------//
    //-----------------------------------------------------------------------//

	[PunRPC]
	public void RemoveCardsResponse (byte[] playerBytes, byte[] chosenCardsBytes) {
        PrepareRPC ();
        List<Adventure> chosenCards = (List<Adventure>)Deserialize(chosenCardsBytes);
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
		if (board.getCardInPlay().IsQuest()) {
			((Quest)board.getCardInPlay ()).getCurrentStage ().DealCardsNextPlayer ();
		} else if (board.getCardInPlay ().GetType () == typeof(QueensFavor)) {
			((QueensFavor)board.getCardInPlay ()).DealCardsNextPlayer ();
		}
		else if (board.getCardInPlay ().GetType () == typeof(ProsperityThroughoutTheRealm)) {
			((ProsperityThroughoutTheRealm)board.getCardInPlay ()).DealCardsNextPlayer ();
		}
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
	public void EvaluateNextPlayerForFoe(bool playerEliminated) {
		PrepareRPC ();
		((Quest)board.getCardInPlay ()).getCurrentStage ().EvaluateNextPlayerForFoe (playerEliminated);
	}
		
	[PunRPC]
	public void PromptEnterTest(byte[] playerBytes, int currentBid) {
		PrepareRPC ();
		Player tempPlayer = (Player)Deserialize (playerBytes);
		Player localPlayer = FindLocalPlayer (tempPlayer);
		board.PromptEnterTest ((Quest)board.getCardInPlay (), localPlayer, currentBid);
	}

    [PunRPC]
    public void PromptTestResponse(bool dropOut, int interactionBid){
        PrepareRPC();
        ((Quest)board.getCardInPlay()).getCurrentStage().PromptTestResponse(dropOut, interactionBid);
    }

	[PunRPC]
	public void PlayStage() {
		PrepareRPC ();
		((Quest)board.getCardInPlay ()).PlayStage ();
	}

    //------------------------------------------------------------------------//
    //------------------------- Tournament Functions -------------------------//
    //------------------------------------------------------------------------//

    [PunRPC]
	public void CardsSelectionResponse(byte[] cardBytes)
    {
		PrepareRPC ();
		List<Adventure> cards = (List<Adventure>) Deserialize (cardBytes);
        ((Tournament)board.getCardInPlay()).CardsSelectionResponse(cards);
    }

    [PunRPC]
    public void PromptEnterTournamentResponse(bool entered)
    {
		PrepareRPC ();
        ((Tournament)board.getCardInPlay()).PromptEnterTournamentResponse(entered);
    }

	[PunRPC]
	public void DisplayTournamentResults() {
		PrepareRPC ();
		((Tournament)board.getCardInPlay ()).DisplayTournamentResultsResponse ();
	}


    //------------------------------------------------------------------------//
    //------------------------- Event Functions ------------------------------//
    //------------------------------------------------------------------------//

    [PunRPC]
    public void PlayerDiscardedWeapon()
    {
        PrepareRPC();
        ((KingsCallToArms)board.getCardInPlay()).PlayerDiscardedWeapon();
    }

    [PunRPC]
    public void PlayerDiscardedFoes()
    {
        PrepareRPC();
        ((KingsCallToArms)board.getCardInPlay()).PlayerDiscardedFoes();
    }

    [PunRPC]
    public void CallToArmsPromptNextPlayer()
    {
        PrepareRPC();
        ((KingsCallToArms)board.getCardInPlay()).PromptNextPlayer();
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