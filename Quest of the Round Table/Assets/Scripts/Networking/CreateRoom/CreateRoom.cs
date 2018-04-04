using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private Text _roomName;
    private Text RoomName{
        get { return _roomName; }
    }

    public void OnClick_CreateRoom(){
	
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
	
        if(PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default)){
            print("Create room successful " + RoomName.text);
        }
        else {
            print("Create room failed to send");
        }
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMessage){
        print("create room failed: " + codeAndMessage[1]);
    }

    void OnCreatedRoom() {
        print("Room created successfully");
    }
}
