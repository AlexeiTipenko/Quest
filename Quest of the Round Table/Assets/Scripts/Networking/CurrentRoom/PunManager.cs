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

	//purely for cheating; as next turn usually gets called through another method
	[PunRPC]
	public void nextTurn () {
		GetBoard ();
		board.nextTurn ();
	}

	[PunRPC]
	public void RemoveCardsResponse (List<Card> chosenCards) {
		GetBoard ();
		board.getCurrentPlayer ().RemoveCardsResponse (chosenCards);
	}

	[PunRPC]
	public void DealCardsNextPlayer() {
		GetBoard ();
		((Quest)board.getCardInPlay()).getCurrentStage().DealCardsNextPlayer();
	}

	[PunRPC]
	public void PromptNextPlayer() {
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
    public void PromptAcceptQuestResponse() {
        GetBoard();
        ((Quest)board.getCardInPlay()).IncrementSponsor();
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

    void GetBoard() {
        board = BoardManagerMediator.getInstance();
    }
}
