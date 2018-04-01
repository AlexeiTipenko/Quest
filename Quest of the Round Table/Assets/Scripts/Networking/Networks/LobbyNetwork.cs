using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
	}
	
    void OnConnectedToMaster() {
        print("Connected to master.");

        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    void OnJoinedLobby(){
        print("Joined lobby");
    }
}
