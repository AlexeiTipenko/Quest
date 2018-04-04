using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunManager : Photon.MonoBehaviour {

    BoardManagerMediator board;


    [PunRPC]
    public void SwitchScene(string seed, string sceneName)
    {
        PlayerLayoutGroup.SwitchScene(seed, sceneName);
    }

    [PunRPC]
    public void PromptEnterTournament(Tournament tournament, Player player, bool entered)
    {
        BoardManager.DrawCover(player);
        tournament.PromptEnterTournamentResponse(entered);
    }

    [PunRPC]
    public void CardsSelectionResponse(Tournament tournament, Player player)
    {
        BoardManager.DrawCover(player);
        tournament.CardsSelectionResponse();
    }

    public void GetBoard(){
        board = BoardManagerMediator.getInstance();
    }

    
}
