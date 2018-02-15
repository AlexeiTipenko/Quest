using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardManager = null;

	void Awake () {

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		print ("Passing playerList to BoardManager");

		foreach (Player player in ButtonManager.playerList) {
			print (player.toString ());
		}

		BoardManagerMediator.getInstance().initGame (ButtonManager.playerList);
		//BoardManagerData.gameLoop (); // uncomment this when the game loop is implemented
								  // in such a way that we don't get into an immediate infinite loop
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
