using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLayoutGroup : MonoBehaviour {

	[SerializeField]
    public GameObject _playerListingPrefab;
    public GameObject PlayerListingPrefab {
        get { return _playerListingPrefab; }
    }

    [NonSerialized]PhotonView view;

    public static List<Player> playerList;

    public static List<PlayerListing> _playerListings = new List<PlayerListing>();
    public static List<PlayerListing> PlayerListings {
        get { return _playerListings; }
    }

    private void OnJoinedRoom() {
        GameObject lobby = GameObject.Find("Canvas/Lobby");
        lobby.SetActive(false);
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++){
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

	public void SetupAI1()
	{
		int aiID = UnityEngine.Random.Range (1, 50);
		view = PhotonView.Get (GameObject.Find ("DDOL/PunManager"));
		view.RPC("AddAI", PhotonTargets.All, 1, aiID);
	}

    public void SetupAI2()
    {
		int aiID = UnityEngine.Random.Range (1, 50);
		view = PhotonView.Get (GameObject.Find ("DDOL/PunManager"));
		view.RPC("AddAI", PhotonTargets.All, 2, aiID);
    }

	public void AddAI(int aiNum, int aiID) {
		GameObject playerListingObj = Instantiate(PlayerListingPrefab);
		Text[] texts = playerListingObj.transform.GetComponentsInChildren<Text>();
		string aiName = "AI_";
		if (aiNum == 1) {
			aiName += "ONE_";
		} else if (aiNum == 2) {
			aiName += "TWO_";
		}
		texts[0].text = aiName + aiID;
		playerListingObj.name = aiName + aiID;
		playerListingObj.transform.SetParent(transform, false);

		PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
		PlayerListings.Add(playerListing);
	}

    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer){
        PlayerJoinedRoom(photonPlayer);
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer) {
        if (photonPlayer == null){
            return;   
        }
        PlayerLeftRoom(photonPlayer);
        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.name = photonPlayer.NickName;
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);

		if (photonPlayer.IsMasterClient) {
			PhotonView view = PhotonView.Get (GameObject.Find ("DDOL/PunManager"));
			System.Random rand = new System.Random();
			string seedString = "";
			for (int i = 0; i < 5; i++) {
				seedString += rand.Next(0, 10);
			}
			view.RPC ("UpdateSeed", PhotonTargets.All, Int32.Parse(seedString));
		}
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer){
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    public void PunSwitchScene(string sceneName) {
        PhotonView view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));
        view.RPC("SwitchScene", PhotonTargets.All, sceneName);
    }

    public void PunSwitchScene1(string sceneName) {
        PhotonView view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));
        view.RPC("SwitchSceneScenario", PhotonTargets.All, sceneName, 1);
    }

	public void PunSwitchScene2(string sceneName) {
		PhotonView view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));
		view.RPC("SwitchSceneScenario", PhotonTargets.All, sceneName, 2);
	}

	public void PunSwitchScene3(string sceneName) {
		PhotonView view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));
		view.RPC("SwitchSceneScenario", PhotonTargets.All, sceneName, 3);
	}

    public static void SwitchScene(string SceneName) {
        playerList = new List<Player>();
        foreach (var player in PlayerListings)
        {
            print("Player name is: " + player.name);
			if (player.name.Contains ("AI_ONE_")) {
				playerList.Add (new AIPlayer (player.name, new Strategy1 ()));
			} else if (player.name.Contains ("AI_TWO_")) {
				playerList.Add (new AIPlayer (player.name, new Strategy2 ()));
			}
            else {
                playerList.Add(new HumanPlayer(player.name));
            }
        }
        ButtonManager.playerList = playerList;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

	public static void SwitchSceneScenario(string SceneName, int scenarioNum){
        playerList = new List<Player>();
        ButtonManager.scenario = "scenario" + scenarioNum;
        foreach (var player in PlayerListings)
        {
            playerList.Add(new HumanPlayer(player.name));
        }
        ButtonManager.playerList = playerList;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
