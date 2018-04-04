using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PunManager : Photon.MonoBehaviour {

    BoardManagerMediator board;

    [PunRPC]
    public void SwitchScene(string seed, string sceneName)
    {
        PlayerLayoutGroup.SwitchScene(seed, sceneName);
    }

    //-----------------------------------------------------------------------//
    //--------------------------- Quest Functions ---------------------------//
    //-----------------------------------------------------------------------//

	//purely for cheating; as next turn usually gets called through another method
	[PunRPC]
	public void nextTurn () {
		GetBoard ();
		board.nextTurn ();
	}

	[PunRPC]
	public void RemoveCardsResponse (byte[] playerBytes, byte[] chosenCardsBytes) {
		GetBoard ();
        List<Card> chosenCards = (List<Card>)Deserialize(chosenCardsBytes);
        Player player = (Player)Deserialize(playerBytes);
		player.RemoveCardsResponse (chosenCards);
	}

	[PunRPC]
	public void DealCardsNextPlayer() {
		GetBoard ();
		((Quest)board.getCardInPlay()).getCurrentStage().DealCardsNextPlayer();
	}

	[PunRPC]
	public void PromptNextPlayer() {
        GetBoard();
		((Tournament)board.getCardInPlay()).PromptNextPlayer();
	}

    [PunRPC]
    public void PromptSponsorQuestResponse(bool sponsorAccepted) {
        GetBoard();
        ((Quest)board.getCardInPlay()).PromptSponsorQuestResponse(sponsorAccepted);
    }

    [PunRPC]
    public void SponsorQuestComplete(byte[] stagesBytes) {
        GetBoard();
        List<Stage> stages = (List<Stage>)Deserialize(stagesBytes);
        ((Quest)board.getCardInPlay()).SponsorQuestComplete(stages);
    }

    [PunRPC]
    public void IncrementSponsor() {
        GetBoard();
        ((Quest)board.getCardInPlay()).IncrementSponsor();
    }

    [PunRPC]
    public void PromptAcceptQuestResponse(bool questAccepted) {
        GetBoard();
        ((Quest)board.getCardInPlay()).PromptAcceptQuestResponse(questAccepted);
    }

    [PunRPC]
    public void PromptNextAcceptQuest() {
        GetBoard();
        ((Quest)board.getCardInPlay()).PromptNextAcceptQuest();
    }

    //------------------------------------------------------------------------//
    //------------------------- Tournament Functions -------------------------//
    //------------------------------------------------------------------------//

    [PunRPC]
    public void CardsSelectionResponse(Tournament tournament)
    {
		GetBoard ();
        tournament.CardsSelectionResponse();
    }

	[PunRPC]
	public void PromptEnterTournamentResponse(Tournament tournament, bool entered)
	{
		GetBoard ();
		tournament.PromptEnterTournamentResponse(entered);
	}

    void GetBoard(){
        board = BoardManagerMediator.getInstance();
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