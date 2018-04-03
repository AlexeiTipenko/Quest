using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameManager : MonoBehaviour {

	public static NetworkGameManager instance = null;
	public BoardManager boardManager = null;

	void Awake () {

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);




		print ("Passing playerList to BoardManager");
        Logger.getInstance().info("Player list passed to BoardManager");

  //      foreach (Player player in PlayerLayoutGroup.NetworkPlayers) {
		//	print (player.toString ());
  //          Logger.getInstance().info("Player entered: " + player.toString());
		//}

		//BoardManagerMediator.getInstance().initGame (ButtonManager.playerList);
		//BoardManagerMediator.getInstance ().startGame ();
	}
	
}
