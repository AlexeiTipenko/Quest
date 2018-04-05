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

    //-----------------------------------------------------------------------//
    //--------------------------- Quest Functions ---------------------------//
    //-----------------------------------------------------------------------//

	//purely for cheating; as next turn usually gets called through another method
	[PunRPC]
	public void nextTurn () {
        PrepareRPC ();
		board.nextTurn ();
	}

	[PunRPC]
	public void RemoveCardsResponse (byte[] playerBytes, byte[] chosenCardsBytes) {
        PrepareRPC ();
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

    void PrepareRPC(){
        board = BoardManagerMediator.getInstance();
        BoardManager.ClearInteractions();
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