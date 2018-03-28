using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkingScript : MonoBehaviour {

    List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
    bool matchCreated;
    NetworkMatch networkMatch;

    void Awake () {
        networkMatch = gameObject.AddComponent<NetworkMatch>();	
	}
	
	public void CreateRoom () {
        string matchName = "room";
        uint matchSize = 4;
        bool matchAdvertise = true;
        string matchPassword = "";
        networkMatch.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreate);
	}

    public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            matchCreated = true;
            NetworkServer.Listen(matchInfo, 9000);
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
        }
        else
        {
            Debug.LogError("Create match failed: " + extendedInfo);
        }
    }

    public void JoinRoom() {
        networkMatch.ListMatches(0, 4, "", true, 0, 0, OnMatchList);
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && matches != null && matches.Count > 0)
        {
            networkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
        }
        else if (!success)
        {
            Debug.LogError("List match failed: " + extendedInfo);
        }
    }

    public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Join match succeeded");
            if (matchCreated)
            {
                Debug.LogWarning("Match already set up, aborting...");
                return;
            }
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
            NetworkClient myClient = new NetworkClient();
            myClient.RegisterHandler(MsgType.Connect, OnConnected);
            myClient.Connect(matchInfo);
        }
        else
        {
            Debug.LogError("Join match failed " + extendedInfo);
        }
    }

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }
}
