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
        PlayerLayoutGroup.SwitchScene(sceneName);
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
        Player tempPlayer = (Player)Deserialize(playerBytes);
		Logger.getInstance ().info ("Received player: " + tempPlayer.getName());
		List<Player> players = board.getPlayers ();
		foreach (Player player in players) {
			if (tempPlayer.getName () == player.getName ()) {
				Logger.getInstance ().info ("Found matching name");
				player.RemoveCardsResponse (chosenCards);
				break;
			}
		}
		Logger.getInstance ().info ("Completed RemoveCardsResponse");
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
        List<Player> players = board.getPlayers();
        Player sponsorPlayer = ((Quest)board.getCardInPlay()).getSponsor();
        foreach(Player player in players){
            if(player.getName() == sponsorPlayer.getName()){
                Debug.Log("Found sponsor " + sponsorPlayer.getName());
                Logger.getInstance().info("Found sponsor " + player.getName());
                ((Quest)board.getCardInPlay()).SponsorQuestComplete(stages);
                break;
            }
        }
        //((Quest)board.getCardInPlay()).SponsorQuestComplete(stages);
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

        //List<Player> players = board.getPlayers();
        //foreach (Player player in players)
        //{
        //    if (player.getName() == sponsorPlayer.getName())
        //    {
        //        Debug.Log("Found sponsor " + sponsorPlayer.getName());
        //        Logger.getInstance().info("Found sponsor " + player.getName());
        //        ((Quest)board.getCardInPlay()).PromptNextAcceptQuest();
        //        break;
        //    }
        //}
        ((Quest)board.getCardInPlay()).PromptNextAcceptQuest();
    }

    //------------------------------------------------------------------------//
    //------------------------- Tournament Functions -------------------------//
    //------------------------------------------------------------------------//

    [PunRPC]
    public void CardsSelectionResponse()
    {
		GetBoard ();
        ((Tournament)board.getCardInPlay()).CardsSelectionResponse();
    }

    /*
	[PunRPC]
	public void PromptEnterTournamentResponse(bool entered)
	{
		GetBoard ();
<<<<<<< HEAD
		tournament.PromptEnterTournamentResponse(entered);
	}
	*/

    [PunRPC]
    public void PromptEnterTournamentResponse(bool entered)
    {
        GetBoard();
		Debug.Log ("board.cardinplay in RPC promptentertournamentresponse is " + board.getCardInPlay().getCardName());
        ((Tournament)board.getCardInPlay()).PromptEnterTournamentResponse(entered);
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