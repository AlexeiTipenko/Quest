using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunManager : Photon.MonoBehaviour {

    BoardManagerMediator board = BoardManagerMediator.getInstance();

    [PunRPC]
    public void SwitchScene(string seed, string sceneName)
    {
        PlayerLayoutGroup.SwitchScene(seed, sceneName);
    }

    [PunRPC]
    public void PromptSponsorQuestResponse(bool sponsorAccepted) {
        ((Quest)board.getCardInPlay()).PromptSponsorQuestResponse(sponsorAccepted);
    }

    [PunRPC]
    public void SponsorQuestComplete(List<Stage> stages) {
        ((Quest)board.getCardInPlay()).SponsorQuestComplete(stages);
    }

    [PunRPC]
    public void IncrementSponsor() {
        ((Quest)board.getCardInPlay()).IncrementSponsor();
    }

    [PunRPC]
    public void PromptAcceptQuestResponse() {
        ((Quest)board.getCardInPlay()).IncrementSponsor();
    }
}
