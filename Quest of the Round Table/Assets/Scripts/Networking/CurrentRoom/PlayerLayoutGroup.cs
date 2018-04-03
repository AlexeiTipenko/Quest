using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLayoutGroup : Photon.MonoBehaviour {

	[SerializeField]
    public GameObject _playerListingPrefab;
    public GameObject PlayerListingPrefab{
        get { return _playerListingPrefab; }
    }
    public static List<PlayerListing> _playerListings = new List<PlayerListing>();
    public static List<PlayerListing> PlayerListings {
        get { return _playerListings; }
    }

    private void OnJoinedRoom(){
        GameObject lobby = GameObject.Find("Canvas/Lobby");
        lobby.SetActive(false);
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++){
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    public void SetupAI1()
    {
        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        Text[] texts = playerListingObj.transform.GetComponentsInChildren<Text>();
        texts[0].text = "AI_One" + Random.Range(1, 50);
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

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer){
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
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer){
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }


    public void PunSwitchScene(string SceneName){
        PhotonView view = PhotonView.Get(GameObject.Find("DDOL/PunManager"));
        view.RPC("SwitchScene", PhotonTargets.All, SceneName);
    }
}
