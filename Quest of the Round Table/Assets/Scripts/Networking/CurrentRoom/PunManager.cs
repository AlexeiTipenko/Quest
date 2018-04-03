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
    public void PromptSponsorQuestResponse(bool sponsorAccepted) {
        GetBoard();
        ((Quest)(board.getCardInPlay())).PromptSponsorQuestResponse(sponsorAccepted);
    }

    void GetBoard() {
        board = BoardManagerMediator.getInstance();
    }
}
